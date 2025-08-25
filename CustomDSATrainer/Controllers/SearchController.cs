using Asp.Versioning;
using CustomDSATrainer.Domain;
using CustomDSATrainer.Domain.ApiResponse;
using CustomDSATrainer.Domain.Interfaces.Repositories;
using CustomDSATrainer.Domain.Interfaces.Services;
using CustomDSATrainer.Domain.Interfaces.UnitOfWork;
using CustomDSATrainer.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CustomDSATrainer.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{v:apiVersion}/search")]
    [EnableRateLimiting("Fixed")]
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [MapToApiVersion(1)]
        [HttpGet("{query}")]
        public async Task<IActionResult> SearchQuery(string query)
        {
            List<int> results = await _unitOfWork.ProblemRepository.GetFromSearch(query);

            Search search = new Search { Id = 0, Query = query, Results = string.Join(", ", results.ToArray())};

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SearchRepository.SaveToDatabase(search);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch { await _unitOfWork.RollbackTransactionAsync(); }

            return Ok(new SearchDTO
            {
                Query = query,
                Results = results,
                TimeOfSearch = DateTime.Now.Date
            });
        }
    }
}
