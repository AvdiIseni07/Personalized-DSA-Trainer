namespace CustomDSATrainer.Domain.Enums
{
    public enum SubmissionResult
    {
        Success,
        Failed,
        CompilationError,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        FailedToLocateInput,
        FailedToLocateOutput
    }
}
