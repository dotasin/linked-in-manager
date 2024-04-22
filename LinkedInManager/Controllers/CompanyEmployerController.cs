using LinkedInManager.Service;
using Microsoft.AspNetCore.Mvc;

namespace LinkedInManager.Controllers
{
    [Route("api/company-employer")]
    [ApiController]
    public class CompanyEmployerController : Controller
    {
        readonly ICompanyEmployerService _companyEmployerService;
        readonly ILogger<CompanyEmployerController> _logger;
        public CompanyEmployerController(ICompanyEmployerService companyEmployerService, ILogger<CompanyEmployerController> logger)
        {
            _companyEmployerService = companyEmployerService;
            _logger = logger;
        }

        [HttpPost("import-employers")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest("Invalid file");

            await _companyEmployerService?.ImportCsv(file);

            return Ok("Employees imported successfully");
        }
    }
}
