namespace LinkedInManager.Models
{
    public class EditLinkedInEmployeeRequest
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string phoneNumberType { get; set; }
        public string linkedInUrl { get; set; }
        public int ranking { get; set; }
        public string headline { get; set; }
        public string seniority { get; set; }
        public string title { get; set; }
        public string searchTechnologies { get; set; }
    }
}
