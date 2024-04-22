using LinkedInManager.Entities;
using static LinkedInManager.Service.LinkedInPeopleService;

namespace LinkedInManager.Service
{
    public interface ILinkedInPeopleService
    {
        Task<PeopleResult> GetSearchedLinkedInEmployeesByFilter(string filter);
        Task<LinkedInEmployee> UpdateLinkedInEmployee(LinkedInEmployee updatedLNEmployee);
        Task<LinkedInEmployee> DeleteLinkedInEmployee(int id);
    }
}