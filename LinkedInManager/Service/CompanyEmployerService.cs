using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Helper;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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

        public async Task<EmployerResult> ImportCsv(IFormFile file)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());

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
