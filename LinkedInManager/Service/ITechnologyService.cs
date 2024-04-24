using LinkedInManager.Entities;
using static LinkedInManager.Service.TechnologyService;

namespace LinkedInManager.Service
{
    public interface ITechnologyService
    {
        List<Technology> GetAllTechnologies();
        Task<TechnologyResult> ImportTechnologies(IFormFile file);
    }
}
