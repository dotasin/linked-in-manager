using LinkedInManager.Data;
using LinkedInManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedInManager.Service
{
    public class LinkedInPeopleService : ILinkedInPeopleService
    {
        private readonly DataContext _context;

        public LinkedInPeopleService(DataContext context)
        {
            _context = context;
        }

        public record PeopleResult(List<LinkedInEmployee> LinkedInEmployees);

        public async Task<PeopleResult> GetSearchedLinkedInEmployeesByFilter(string filter)
        {
            var listOfPeople = await _context.LinkedInEmployees.ToListAsync();
            var filtered = listOfPeople.Where(x => x.SearchTechnologies.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.State.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.City.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.Seniority.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                 x.Headline.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
            return new PeopleResult(filtered);
        }

        public async Task<LinkedInEmployee> UpdateLinkedInEmployee(LinkedInEmployee updatedLNEmployee)
        {
            var dbLinkedInEmployee = await _context.LinkedInEmployees.FindAsync(updatedLNEmployee.Id);

            dbLinkedInEmployee.FirstName = updatedLNEmployee.FirstName;
            dbLinkedInEmployee.LastName = updatedLNEmployee.LastName;
            dbLinkedInEmployee.Name = updatedLNEmployee.Name;
            dbLinkedInEmployee.State = updatedLNEmployee.State;
            dbLinkedInEmployee.City = updatedLNEmployee.City;
            dbLinkedInEmployee.Country = updatedLNEmployee.Country;
            dbLinkedInEmployee.Email = updatedLNEmployee.Email;
            dbLinkedInEmployee.PhoneNumber = updatedLNEmployee.PhoneNumber;
            dbLinkedInEmployee.PhoneNumberType = updatedLNEmployee.PhoneNumberType;
            dbLinkedInEmployee.LinkedInUrl = updatedLNEmployee.LinkedInUrl;
            dbLinkedInEmployee.Ranking = updatedLNEmployee.Ranking;
            dbLinkedInEmployee.Headline = updatedLNEmployee.Headline;
            dbLinkedInEmployee.Seniority = updatedLNEmployee.Seniority;
            dbLinkedInEmployee.SearchTechnologies = updatedLNEmployee.SearchTechnologies;

            await _context.SaveChangesAsync();

            return await _context.LinkedInEmployees.Where(x => x.Id == updatedLNEmployee.Id).FirstOrDefaultAsync();
        }

        public async Task<LinkedInEmployee> DeleteLinkedInEmployee(int id)
        {
            var dbLinkedInEmployee = await _context.LinkedInEmployees.FindAsync(id);

            if (dbLinkedInEmployee == null)
                throw new Exception("Linked In Employee not found");

            _context.LinkedInEmployees.Remove(dbLinkedInEmployee);
            await _context.SaveChangesAsync();

            return await _context.LinkedInEmployees.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
