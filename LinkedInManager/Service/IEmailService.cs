using LinkedInManager.Data;
using LinkedInManager.Models;

namespace LinkedInManager.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(SingleEmailRequest email);

        Task SendEmailToMultipleEmployees(MultipleEmailRequest email);
    }
}
