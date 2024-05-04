using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkedInManager.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/app")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        readonly DataContext _context;
        readonly ILogger<AppUserController> _logger;
        public AppUserController(DataContext context, ILogger<AppUserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<AppUser>>> GetAllAppUsers()
        {
            var appUsers = await _context.AppUsers.ToListAsync();

            return Ok(appUsers);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAppUsersByCriteria([FromQuery] string filter)
        {
            var filteredUsers = await _context.AppUsers
               .Where(u => EF.Functions.Like(u.Email, $"%{filter}%") ||
                        EF.Functions.Like(u.FirstName, $"%{filter}%") ||
                        // Add more properties as needed
                        EF.Functions.Like(u.LastName, $"%{filter}%"))
            .ToListAsync();

            if (!filteredUsers.Any())
                return NotFound("No users matching the filter were found");

            return Ok(filteredUsers);
        }

        [HttpPost("add")]
        public async Task<ActionResult<List<AppUser>>> AddAppUser(AppUser appUser)
        {
            _context.AppUsers.Add(appUser);
            await _context.SaveChangesAsync();

            return Ok(await _context.AppUsers.ToListAsync());
        }

        [HttpPost("update")]
        public async Task<ActionResult<List<AppUser>>> UpdateAppUser(AppUserEditRequest updateAppUser)
        {
            var dbAppUser = await _context.AppUsers.FindAsync(updateAppUser.id);

            if (dbAppUser == null)
                return NotFound("App User not found");

            dbAppUser.FirstName = updateAppUser.firstName;
            dbAppUser.LastName = updateAppUser.lastName;
            dbAppUser.SMTPAppPassword = updateAppUser.smtpAppPassword;
            dbAppUser.Email = updateAppUser.email;
            dbAppUser.ApoloApiKey = updateAppUser.apoloApiKey;

            await _context.SaveChangesAsync();

            return Ok(await _context.AppUsers.ToListAsync());
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<List<AppUser>>> DeleteAppUser(int id)
        {
            var dbUser = await _context.AppUsers.FindAsync(id);

            if (dbUser == null)
                return NotFound("App User not found");

            _context.AppUsers.Remove(dbUser);

            await _context.SaveChangesAsync();

            return Ok(await _context.AppUsers.ToListAsync());
        }
    }
}
