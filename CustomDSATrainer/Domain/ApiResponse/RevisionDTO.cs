using Azure.Core.Pipeline;

namespace CustomDSATrainer.Domain.ApiResponse
{
    public class RevisionDTO
    {
        public int ProblemId { get; set; }
        public string ProblemTitle { get; set; }
        public string ProblemStatement { get; set; }
        public DateTime SelectedAt { get; set; }
    }
}
