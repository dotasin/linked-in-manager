using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkedInManager.Entities
{
    public class AppUser
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SMTPAppPassword { get; set; } = string.Empty;
        public string ApoloApiKey { get; set; } = string.Empty;
    }
}
