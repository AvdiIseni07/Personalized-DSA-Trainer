using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain
{
    public class Search
    {
        public int Id { get; set; }
        public string? Query { get; set; }
        public string? Categories { get; set; }
        public string? Difficulty { get; set; }
        public ProblemStatus? Statuts { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Results { get; set; }
        public DateTime TimeOfSearch { get; set; }
    }
}
