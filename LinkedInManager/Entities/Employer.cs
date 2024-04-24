using System.ComponentModel.DataAnnotations;

namespace LinkedInManager.Entities
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Revenue { get; set; } = string.Empty;
        public string EmployeeCount { get; set; } = string.Empty;
        public bool ValidEmailAddress { get; set; } = false;
    }
}
