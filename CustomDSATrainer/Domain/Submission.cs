using CustomDSATrainer.Application;
using CustomDSATrainer.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomDSATrainer.Domain
{
    public class Submission
    {
        public int Id { get; set; }
        public required string PathToExecutable { get; set; }
        public required string PathToInput { get; set; }
        public SubmissionResult Result { get; set; }
        public float AverageExecutionTime { get; set; }
        public int ProblemId { get; set; }
        public Problem Problem { get; set; }
        private UserSourceLinker _UserSourceLinker { get; set; } = new UserSourceLinker();
        
        public Submission() { }

        public Submission(string pathToExecutable, string pathToInput)
        {
            PathToExecutable = pathToExecutable;
            PathToInput = pathToInput;
        }

        public void RunSumbission()
        {
            this._UserSourceLinker.RunCppExecutable(this.PathToExecutable, this.PathToInput);
        }
    }
}
