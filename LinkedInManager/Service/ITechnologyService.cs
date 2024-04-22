using LinkedInManager.Entities;

namespace LinkedInManager.Service
{
    public interface ITechnologyService
    {
        List<Technology> GetAllTechnologies();
        Task<List<Technology>> ImportTechnologies(IFormFile file);
    }
}
