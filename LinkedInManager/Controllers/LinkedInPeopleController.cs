using LinkedInManager.Entities;
using LinkedInManager.Service;
using Microsoft.AspNetCore.Mvc;
using static LinkedInManager.Service.LinkedInPeopleService;

namespace LinkedInManager.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class LinkedInPeopleController : Controller
    {
        readonly ILinkedInPeopleService _linkedInPeopleService;
        readonly ILogger<CompanyEmployerController> _logger;
        public LinkedInPeopleController(ILinkedInPeopleService linkedInPeopleService, ILogger<CompanyEmployerController> logger)
        {
            _linkedInPeopleService = linkedInPeopleService;
            _logger = logger;
        }

        /// <summary>
        /// This methods pull recordings from database
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public async Task<ActionResult<List<LinkedInEmployee>>> GetAllLinkedInPeople() =>
             Ok(await _linkedInPeopleService.GetSearchedLinkedInEmployeesByFilter(""));

        [HttpGet("filter")]
        public ActionResult<PeopleResult> GetSearchedPeopleByFilter([FromQuery] string filter)
        {
            var filtered = _linkedInPeopleService.GetSearchedLinkedInEmployeesByFilter(filter);

            if (!filtered.Result.LinkedInEmployees.Any())
                return NotFound("No users matching the filter were found");

            return Ok(filtered);
        }

        /// <summary>
        /// This controller will serve to edit row in database for LinkedInEmployeee
        /// </summary>
        /// <param name="updatedLNEmployee"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<ActionResult<List<LinkedInEmployee>>> UpdateLinkedInEmployee(LinkedInEmployee updatedLNEmployee) =>
             Ok(await _linkedInPeopleService.UpdateLinkedInEmployee(updatedLNEmployee));


        /// <summary>
        /// This controller will serve to delete employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ActionResult<LinkedInEmployee>> DeleteLNEmployee(int id) =>
             Ok(await _linkedInPeopleService.DeleteLinkedInEmployee(id));
        

        /// <summary>
        /// This controller will serve to add new 
        /// </summary>
        /// <param name="lnEmployee"></param>
        /// <returns></returns>
        //[HttpPost("add")]
        //public async Task<ActionResult<List<LinkedInEmployee>>> AddLNEmployee(LinkedInEmployee lnEmployee)
        //{
        //    _context.LinkedInEmployees.Add(lnEmployee);
        //    await _context.SaveChangesAsync();

        //    return Ok(await _context.LinkedInEmployees.ToListAsync());
        //}
    }
}
