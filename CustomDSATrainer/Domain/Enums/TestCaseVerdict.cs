namespace CustomDSATrainer.Domain.Enums
{
    public enum TestCaseVerdict
    {
        NoVerdict,
        Passed,
        IncorrectAnswer,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        TooManyArguments,
        TooFewArguments
    }
}