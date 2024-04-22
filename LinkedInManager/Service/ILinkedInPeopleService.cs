using LinkedInManager.Entities;
using static LinkedInManager.Service.LinkedInPeopleService;

namespace LinkedInManager.Service
{
    public interface ILinkedInPeopleService
    {
        Task<PeopleResult> GetSearchedLinkedInEmployeesByFilter(string filter);
        Task<LinkedInPeople> UpdateLinkedInEmployee(LinkedInPeople updatedLNEmployee);
        Task<LinkedInPeople> DeleteLinkedInEmployee(int id);
        Task<ImportExportResult> ImportPeoplesFromDBtoDb(IFormFile file);
        Task<ImportExportResult> ExportPeoplesFromDBtoDb();
    }
}