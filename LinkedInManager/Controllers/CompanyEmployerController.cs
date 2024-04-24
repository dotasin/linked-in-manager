using LinkedInManager.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static LinkedInManager.Service.CompanyEmployerService;

namespace LinkedInManager.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
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


        [HttpGet("filter")]
        public ActionResult<EmployerFilterResult> GetSearchedPeopleByFilter([FromQuery] string filter)
        {
            var filtered = _companyEmployerService.GetImportedEmployersByFilter(filter);
            var result = filtered.Result;

            if (!filtered.Result.employers.Any())
                return NotFound("No linked in people (employee) matching the filter were found");

            return Ok(result);
        }
    }
}
