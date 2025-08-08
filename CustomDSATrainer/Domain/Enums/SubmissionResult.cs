namespace CustomDSATrainer.Domain.Enums
{
    public enum SubmissionResult
    {
        Success,
        CompilationError,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        FailedToLocateInput,
        FailedToLocateOutput
    }
}
