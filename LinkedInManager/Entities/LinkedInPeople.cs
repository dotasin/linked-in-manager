using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkedInManager.Entities
{
    public class LinkedInPeople
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? PhoneNumberType { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; } = string.Empty;
        public int Ranking { get; set; } = 1;
        public string? Headline { get; set; }
        public string? Seniority { get; set; }
        public string? Title { get; set; }
        public int SearchId { get; set; } = 0;
        public bool Imported { get; set; } = false;

        [ForeignKey(nameof(SearchId))]
        public Search? Search { get; set; }
        public string? SearchTechnologies { get; set; }
    }
}
