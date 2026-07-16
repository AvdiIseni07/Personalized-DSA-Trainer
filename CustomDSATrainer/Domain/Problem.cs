using CustomDSATrainer.Domain.Enums;

namespace CustomDSATrainer.Domain
{
    public class Problem
    {
        public required int Id { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Statement { get; set; } = string.Empty;
        public string? Difficulty { get; set; } = string.Empty;
        public string? Categories { get; set; } = string.Empty; // "category1,category2"
        public string? Hint { get; set; } = string.Empty;
        public ProblemStatus Status { get; set; } = ProblemStatus.NotTried;
        public string Inputs { get; set; } = string.Empty;
        public string Outputs { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
      
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
        public ICollection<AIReview> AIReviews { get; set; } = new List<AIReview>();
        public Problem()
        { }
        public Problem(int id, string title, string statement, string difficulty, string categories)
        {
            Id = id;
            Title = title;
            Statement = statement;
            Difficulty = difficulty;
            Categories = categories;
        }        
    }
}
