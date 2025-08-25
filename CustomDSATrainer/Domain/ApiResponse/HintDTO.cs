using System.Security.Permissions;

namespace CustomDSATrainer.Domain.ApiResponse
{
    public class HintDTO
    {
        public string Hint { get; set; }
        public DateTime RevealedAt { get; set; }
    }
}
