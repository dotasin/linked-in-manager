using LinkedInManager.Entities;
using LinkedInManager.Models;
using static LinkedInManager.Service.LinkedInPeopleService;

namespace LinkedInManager.Service
{
    public interface ILinkedInPeopleService
    {
        Task<PeopleResult> GetSearchedLinkedInEmployeesByFilter(string filter);
        Task<LinkedInEditResult> UpdateLinkedInEmployee(EditLinkedInEmployeeRequest updatedLNEmployee);
        Task<LinkedInPeople> DeleteLinkedInEmployee(int id);
        Task<ImportExportResult> ImportPeoplesFromDBtoDb(IFormFile file);
        Task<ImportExportResult> ExportPeoplesFromDBtoDb(List<LinkedInPeople> peoples = null);
    }
}