using LinkedInManager.Entities;
using static LinkedInManager.Service.CompanyEmployerService;

namespace LinkedInManager.Service
{
    public interface ICompanyEmployerService
    {
        Task<EmployerResult> ImportCsv(IFormFile file);
    }
}