namespace CustomDSATrainer.Domain.ApiResponse
{
    public class SearchDTO
    {
        public string Query { get; set; }
        public List<int> Results { get; set; }
        public DateTime TimeOfSearch { get; set; }
    }
}
