using CustomDSATrainer.Domain.Enums;
using CustomDSATrainer.Migrations;
using System.Security.Policy;

namespace CustomDSATrainer.Domain.ApiResponse
{
    public class SearchDTO
    {
        public string? SearchString { get; set; }
        public string? Categories { get; set; }
        public string? Difficulty { get; set; }
        public ProblemStatus? Status { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public DateTime? DateLowerBound { get; set; }
        public DateTime? DateUpperBound { get; set; }
        public SortOption? SortOption { get; set; }
        public DateTime SearchedAt { get; set; }
        public PaginatedList<Problem> Result { get; set; }
    }
}
