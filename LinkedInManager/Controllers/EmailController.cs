using LinkedInManager.Data;
using LinkedInManager.Models;
using LinkedInManager.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LinkedInManager.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        readonly IEmailService _emailService;
        readonly ILogger<CompanyEmployerController> _logger;
        public EmailController(IEmailService emailService, ILogger<CompanyEmployerController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send-single")]
        public async Task<IActionResult> SendEmailAsync([FromBody] SingleEmailRequest email)
        {
            try
            {
                await _emailService.SendEmailAsync(email);

                return Ok("Email sent successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to send email.");
            }
        }
    }
}