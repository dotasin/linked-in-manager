using System.ComponentModel.DataAnnotations;

namespace LinkedInManager.Entities
{
    public class Search
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public int TotalRecords { get; set; }

        public SearchState SearchState { get; set; }

        public string StatusMessage { get; set; } = string.Empty;
    }

    public enum SearchState
    {
        NotStarted = 0, Started = 1, Done = 3, Failed = 4
    }
}
