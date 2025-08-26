using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Drawing.Drawing2D;

namespace CustomDSATrainer.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/search")]
    [EnableRateLimiting("Fixed")]
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Index
            (
                [FromQuery] string? searchString,
                [FromQuery] string? categories,
                [FromQuery] string? difficulty,
                [FromQuery] ProblemStatus? status,
                [FromQuery] int? pageNumber,
                [FromQuery] int? pageSize,
                [FromQuery] DateTime? dateLowerBound,
                [FromQuery] DateTime? dateUpperBound,
                [FromQuery] SortOption sortOption
            )
        {
            if (searchString == null && categories == null && difficulty == null && status == null && pageNumber == null && pageSize == null && dateLowerBound == null && dateUpperBound == null)
                return BadRequest("At least one parameter should be given.");


            var results = await _unitOfWork.ProblemRepository.GetPages(searchString, categories, difficulty, status, pageNumber, pageSize, dateLowerBound, dateUpperBound, sortOption);
            List<int> problems = results.Select(p => p.Id).ToList();

            Search search = new Search 
            {
                Id = 0, 
                Query = searchString, 
                Categories = categories, 
                Difficulty = difficulty, 
                PageNumber = pageNumber, 
                Statuts = status,
                PageSize = pageSize,
                Results = string.Join(", ", problems),
                TimeOfSearch = DateTime.Now.Date
            };

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SearchRepository.SaveToDatabase(search);
                await _unitOfWork.CommitTransactionAsync();
            } catch { await _unitOfWork.RollbackTransactionAsync(); }

            return Ok(new SearchDTO
            {
                SearchString = searchString,
                Categories = categories,
                Difficulty = difficulty,
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize,
                DateLowerBound = dateLowerBound,
                DateUpperBound = dateUpperBound,
                SortOption = sortOption,
                SearchedAt = DateTime.Now,
                Result = results
            });
        }
    }
}
