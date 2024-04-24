using CsvHelper;
using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Models;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace LinkedInManager.Service
{
    public class LinkedInPeopleService : ILinkedInPeopleService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        public LinkedInPeopleService(DataContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public record PeopleResult(List<LinkedInPeople> LinkedInEmployees);

        public async Task<PeopleResult> GetSearchedLinkedInEmployeesByFilter(string filter)
        {
            var filterLower = filter.ToLower(); // Convert filter to lowercase

            var filtered = await _context.LinkedInPeoples
                .Where(x => x.SearchTechnologies.ToLower().Contains(filterLower) ||
                            x.Title.ToLower().Contains(filterLower) ||
                            x.State.ToLower().Contains(filterLower) ||
                            x.City.ToLower().Contains(filterLower) ||
                            x.Seniority.ToLower().Contains(filterLower) ||
                            x.LastName.ToLower().Contains(filterLower) ||
                            x.FirstName.ToLower().Contains(filterLower) ||
                            x.Headline.ToLower().Contains(filterLower)).ToListAsync();

            return new PeopleResult(filtered);
        }

        public record LinkedInEditResult(string message, LinkedInPeople employee = null);
        public async Task<LinkedInEditResult> UpdateLinkedInEmployee(EditLinkedInEmployeeRequest updatedLNEmployee)
        {
            var dbLinkedInEmployee = await _context.LinkedInPeoples.FindAsync(updatedLNEmployee.id);

            if (dbLinkedInEmployee != null)
            {
                dbLinkedInEmployee.FirstName = updatedLNEmployee.firstName;
                dbLinkedInEmployee.LastName = updatedLNEmployee.lastName;
                dbLinkedInEmployee.Name = updatedLNEmployee.name;
                dbLinkedInEmployee.State = updatedLNEmployee.state;
                dbLinkedInEmployee.City = updatedLNEmployee.city;
                dbLinkedInEmployee.Country = updatedLNEmployee.country;
                dbLinkedInEmployee.Email = updatedLNEmployee.email;
                dbLinkedInEmployee.PhoneNumber = updatedLNEmployee.phoneNumber;
                dbLinkedInEmployee.PhoneNumberType = updatedLNEmployee.phoneNumberType;
                dbLinkedInEmployee.LinkedInUrl = updatedLNEmployee.linkedInUrl;
                dbLinkedInEmployee.Ranking = updatedLNEmployee.ranking;
                dbLinkedInEmployee.Headline = updatedLNEmployee.headline;
                dbLinkedInEmployee.Seniority = updatedLNEmployee.seniority;
                dbLinkedInEmployee.Title = updatedLNEmployee.title;
                dbLinkedInEmployee.SearchTechnologies = updatedLNEmployee.searchTechnologies;

                await _context.SaveChangesAsync();
                var edited = await _context.LinkedInPeoples.Where(x => x.Id == updatedLNEmployee.id).FirstOrDefaultAsync();
                return new LinkedInEditResult($"Employee edit success!", edited);
            }
            else
                return new LinkedInEditResult($"There is no employee with id {updatedLNEmployee.id}");
            
        }

        public async Task<LinkedInPeople> DeleteLinkedInEmployee(int id)
        {
            var dbLinkedInEmployee = await _context.LinkedInPeoples.FindAsync(id);

            if (dbLinkedInEmployee == null)
                throw new Exception("Linked In Employee not found");

            _context.LinkedInPeoples.Remove(dbLinkedInEmployee);
            await _context.SaveChangesAsync();

            return await _context.LinkedInPeoples.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public record ImportExportResult(int statusCode, bool success,  string message);
        public async Task<ImportExportResult> ImportPeoplesFromDBtoDb(IFormFile file)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            var peopleFromDb = context.LinkedInPeoples.ToList();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<LinkedInPeopleImportExport>().ToList();

                Search newSearch = new Search
                {
                    CreatedAt = DateTime.Now,
                    SearchState = SearchState.Imported,
                    StatusMessage = "Import from db",
                    TotalRecords = records.Count
                };

                context.Searches.Add(newSearch);
                context.SaveChanges();
                var searchId = context.Searches.ToList().LastOrDefault().Id;
                
                var excludedDuplicates = records.Where(x => !peopleFromDb.Any(p => p.LinkedInUrl == x.LinkedInUrl)).ToList();

                if (excludedDuplicates.Count == 0)
                {
                    return new ImportExportResult(400, false, "All people from import list are already imported.");
                }
                else
                {

                    var list = excludedDuplicates.Select(x => new LinkedInPeople()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Name = x.Name,
                        State = x.State,
                        City = x.City,
                        Country = x.Country,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        PhoneNumberType = x.PhoneNumberType,
                        LinkedInUrl = x.LinkedInUrl,
                        Ranking = x.Ranking,
                        Headline = x.Headline,
                        Seniority = x.Seniority,
                        Title = x.Title,
                        SearchTechnologies = x.SearchTechnologies,
                        Imported = true,
                        SearchId = searchId
                    }).ToList();


                    context.LinkedInPeoples.AddRange(list);
                    await context.SaveChangesAsync();


                    return new ImportExportResult(200, true, "Successfully imported people from db to db!");
                }
            }
        }

        public async Task<ImportExportResult> ExportPeoplesFromDBtoDb(List<LinkedInPeople> peoples)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
    
            try
            { 
                // Transform LinkedInPeople objects into LinkedInPeopleImportExport objects
                var peopleToExport = peoples.Select(p => new LinkedInPeopleImportExport
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Name = p.Name,
                    State = p.State,
                    City = p.City,
                    Country = p.Country,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    PhoneNumberType = p.PhoneNumberType,
                    LinkedInUrl = p.LinkedInUrl,
                    Ranking = p.Ranking,
                    Headline = p.Headline,
                    Seniority = p.Seniority,
                    Title = p.Title,
                    SearchTechnologies = p.SearchTechnologies
                }).ToList();

                // Generate CSV content
                var csvContent = GenerateCsv(peopleToExport);

                // Set file name
                var fileName = "LinkedInPeopleExport.csv";
                var filePath = Path.Combine(@"C:\Publish\LinkedInManager\", fileName);

                // Write CSV content to file
                File.WriteAllText(filePath, csvContent);

                // Mark exported people as Imported
                foreach (var person in peoples)
                {
                    person.Exported = true;
                }

                // Save changes to the database
                await context.SaveChangesAsync();

                // Return file path for download
                return new ImportExportResult(200, true, $"All people are exported to: {filePath}");
            }
            catch (Exception ex)
            {
                return new ImportExportResult(500, false, $"An error occurred while exporting LinkedIn people: {ex.Message}");
            }
        }

        public string GenerateCsv(IEnumerable<LinkedInPeopleImportExport> data)
        {
            StringBuilder sb = new StringBuilder();

            // Write header
            sb.AppendLine("FirstName,LastName,Name,State,City,Country,Email,PhoneNumber,PhoneNumberType,LinkedInUrl,Ranking,Headline,Seniority,Title,SearchTechnologies");

            // Write data rows
            foreach (var person in data)
            {
                sb.AppendLine($"{EscapeCsvField(person.FirstName)},{EscapeCsvField(person.LastName)},{EscapeCsvField(person.Name)},{EscapeCsvField(person.State)},{EscapeCsvField(person.City)},{EscapeCsvField(person.Country)},{EscapeCsvField(person.Email)},{EscapeCsvField(person.PhoneNumber)},{EscapeCsvField(person.PhoneNumberType)},{EscapeCsvField(person.LinkedInUrl)},{person.Ranking},{EscapeCsvField(person.Headline)},{EscapeCsvField(person.Seniority)},{EscapeCsvField(person.Title)},{EscapeCsvField(person.SearchTechnologies)}");
            }

            return sb.ToString();
        }

        private string EscapeCsvField(string field)
        {
            if (field == null)
                return "";

            // If field contains comma, double quotes, or newline, enclose it in double quotes and escape existing double quotes
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            }

            return field;
        }
    }
}
