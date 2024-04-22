﻿using LinkedInManager.Entities;
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
        public async Task<ActionResult<List<LinkedInPeople>>> GetAllLinkedInPeople() =>
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
        public async Task<ActionResult<List<LinkedInPeople>>> UpdateLinkedInEmployee(LinkedInPeople updatedLNEmployee) =>
             Ok(await _linkedInPeopleService.UpdateLinkedInEmployee(updatedLNEmployee));


        /// <summary>
        /// This controller will serve to delete employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<ActionResult<LinkedInPeople>> DeleteLNEmployee(int id) =>
             Ok(await _linkedInPeopleService.DeleteLinkedInEmployee(id));


        [HttpPost("import-")]
        public async Task<IActionResult> ImportTags(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return BadRequest("Invalid file");

            _linkedInPeopleService?.ImportPeoplesFromDBtoDb(file);

            return Ok("Technologies imported successfully");
        }
    }
}
