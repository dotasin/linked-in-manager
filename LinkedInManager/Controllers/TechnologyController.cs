using LinkedInManager.Entities;
using LinkedInManager.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;


namespace LinkedInManager.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/apolo")]
    [ApiController]
    public class TechnologyController : Controller
    {
        readonly ITechnologyService _technologyService;
        readonly ILogger<CompanyEmployerController> _logger;
        public TechnologyController(ITechnologyService technologyService, ILogger<CompanyEmployerController> logger)
        {
            _technologyService = technologyService;
            _logger = logger;
        }

        [HttpGet("get-all-technologies")]
        public List<Technology> GetAllTechnologies() =>
             _technologyService.GetAllTechnologies();

        [HttpPost("import")]
        public async Task<IActionResult> ImportTags(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest("Invalid file");

            _technologyService?.ImportTechnologies(file);

            return Ok("Technologies imported successfully");
        }
    }
}
