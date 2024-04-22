namespace LinkedInManager.Models
{
    public class SingleEmailRequest
    {
        public string? SenderEmail { get; set; }
        public string? RecipientEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }

    public class MultipleEmailRequest
    {
        public string? SenderEmail { get; set; }
        public List<string>? RecipientEmails { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
