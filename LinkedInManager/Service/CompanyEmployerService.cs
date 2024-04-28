using CsvHelper;
using CsvHelper.Configuration;
using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Helper;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static LinkedInManager.Helper.Utils;

namespace LinkedInManager.Service
{
    public class CompanyEmployerService : ICompanyEmployerService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;

        public CompanyEmployerService(DataContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }
        public record EmployerResult(List<Employer> employers, string message, bool success);
        public record EmployerFilterResult(string message, List<Employer> employers);
        public async Task<EmployerResult> ImportCsv(IFormFile file)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            var employersFromDb = await context.Employers.ToListAsync();
            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null // Ignore header validation
                }))
                {
                    csv.Context.TypeConverterCache.AddConverter<decimal?>(new ScientificNotationConverter());

                    var records = csv.GetRecords<EmployerCsv>().ToList();

                    var listOfEmployers = new List<Employer>();

                    //to do za sad ne postoji nacin 
                    //var excludedDuplicates = records.Where(x => !employersFromDb.Any(p => p. == x.LinkedInUrl)).ToList();

                    foreach (var record in records)
                    {
                        // Set default values for fields that might be missing
                        string revenue = string.IsNullOrWhiteSpace(record.revenue) ? record.revenue.ToString() : string.Empty;
                        string employeeCount = string.IsNullOrWhiteSpace(record.employee_count) ? "0" : record.employee_count;

                        // Create a new Employer object and populate its properties
                        var employer = new Employer()
                        {
                            FirstName = record.first_name,
                            LastName = record.last_name,
                            JobTitle = record.job_title,
                            CompanyName = record.company_name,
                            Address = record.address,
                            City = record.city,
                            State = record.state,
                            Email = record.email,
                            Domain = record.domain,
                            PhoneNumber = record.phone_number,
                            Revenue = revenue,
                            EmployeeCount = employeeCount.ToString(),
                            ValidEmailAddress = Utils.IsValidEmailDomain(record.email)
                        };


                        if (employer.ValidEmailAddress)
                            listOfEmployers.Add(employer);
                    }

                    listOfEmployers.Count();
                    // Add the list of employers to the database context and save changes
                    context.Employers.AddRange(listOfEmployers);

                    await context.SaveChangesAsync();

                }

                return new EmployerResult(await _context.Employers.ToListAsync(), "Successfull addad list of employers with valid mail domain", true);
            }
            catch (Exception ex)
            {
                return new EmployerResult(new List<Employer>(), $"Import Failed {ex.Message}", false);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EmployerFilterResult> GetImportedEmployersByFilter(string filter)
        {
            var listOfPeople = await _context.Employers.ToListAsync();
            if (!string.IsNullOrWhiteSpace(filter)) 
            {
                var filtered = listOfPeople.Where(x => x.FirstName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.LastName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.State.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.JobTitle.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.CompanyName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Address.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.City.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.State.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Email.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Domain.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.PhoneNumber.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Revenue.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                                                     x.EmployeeCount.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();
                if (filtered.Count == 0)
                    return new EmployerFilterResult($"There is no employers by filter criteria [{filter}]", filtered);
                else
                    return new EmployerFilterResult("Success filtered employers!", filtered);
            }
            else
                return new EmployerFilterResult("Success filtered employers!", listOfPeople);
        }


        /// <summary>
        /// first_name,last_name,job_title,company_name,address,city,state,zip_code,email,basedomain,phone_number,reveneuw,employee_count
        /// </summary>
        public record EmployerCsv()
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string job_title { get; set; }
            public string company_name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip_code { get; set; }
            public string email { get; set; }
            public string domain { get; set; }
            public string phone_number { get; set; }
            public string revenue { get; set; }
            public string employee_count { get; set; }
        }
    }
}
