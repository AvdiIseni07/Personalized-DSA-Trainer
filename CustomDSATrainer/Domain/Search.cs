namespace CustomDSATrainer.Domain
{
    public class Search
    {
        public int Id { get; set; }
        public string Query { get; set; } = string.Empty;
        public string Results { get; set; } = string.Empty;
    }
}
