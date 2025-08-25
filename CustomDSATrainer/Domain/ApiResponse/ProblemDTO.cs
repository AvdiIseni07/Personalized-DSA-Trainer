using Azure.Core.Pipeline;

namespace CustomDSATrainer.Domain.ApiResponse
{
    public class ProblemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Statement { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
