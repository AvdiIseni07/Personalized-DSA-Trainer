using CustomDSATrainer.Domain.Enums;
using System.Runtime.CompilerServices;

namespace CustomDSATrainer.Domain
{
    public class AIReview
    {
        public int Id { get; set; }
        public required string PathToCPPFile { get; set; }
        public string Review { get; set; } = string.Empty;
        public ProblemStatus ProblemStatus { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        public AIReview() { }
        public AIReview(string pathToCPPFile, ProblemStatus problemStatus)
        {
            PathToCPPFile = pathToCPPFile;
            ProblemStatus = problemStatus;
        }

    }
}
