namespace LinkedInManager.Models
{
    public class AppUserEditRequest
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string smtpAppPassword { get; set; }
        public string apoloApiKey { get; set; }
    }
}
