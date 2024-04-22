namespace LinkedInManager.Entities
{
    public class LinkedInPeopleImportExport
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public string? Name { get; set; }
        public string? State { get; set; } 
        public string? City { get; set; } 
        public string? Country { get; set; } 
        public string? Email { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string? PhoneNumberType { get; set; } 
        public string? LinkedInUrl { get; set; }
        public int Ranking { get; set; } = 1;
        public string? Headline { get; set; }
        public string? Seniority { get; set; }
        public string? Title { get; set; }
        public string? SearchTechnologies { get; set; }
    }
}
