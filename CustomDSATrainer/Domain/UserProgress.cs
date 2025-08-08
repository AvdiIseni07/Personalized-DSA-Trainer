namespace CustomDSATrainer.Domain
{
    public class UserProgress
    {
        public int Id { get; set; }
        public uint TotalAttemptedTasks { get; set; } = 0;
        public uint TotalSolvedProblems { get; set; } = 0;
        public uint TotalUnsolvedProblems { get; set; } = 0;
        public uint DaysLogged { get; set; } = 0;
        public uint LoggingStreak { get; set; } = 0;
        public string PerviousDayLog { get; set; } = string.Empty;
    }
}
