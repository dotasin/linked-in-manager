using System.Net.Mail;
using System.Net;
using LinkedInManager.Models;
using LinkedInManager.Data;
using LinkedInManager.Helper;

namespace LinkedInManager.Service
{
    public class EmailService: IEmailService
    {
        private readonly DataContext _context;

        public EmailService(DataContext context)
        {
            _context = context;
        }
        public async Task SendEmailAsync(SingleEmailRequest email)
        {
            try
            {
                var emailPass = _context.AppUsers.FirstOrDefault(u => u.Email == email.SenderEmail).SMTPAppPassword;

                if (!Utils.IsValidEmailDomain(email.RecipientEmail))
                    throw new Exception("Mail domain is invalid.");

                var message = new MailMessage(email.SenderEmail, email.RecipientEmail);
                message.Subject = email.Subject;
                message.Body = email.Body;

                var client = new SmtpClient("smtp.gmail.com");
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                var credentials = new NetworkCredential(email.SenderEmail, emailPass);
                client.Credentials = credentials;

                await client.SendMailAsync(message);

                message.Dispose();
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task SendEmailToMultipleEmployees(MultipleEmailRequest email)
        {
            throw new NotImplementedException();
        }
    }
}
