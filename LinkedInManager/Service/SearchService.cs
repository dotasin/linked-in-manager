using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Models;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace LinkedInManager.Service
{
    public class SearchService : ISearchService
    {
        //private readonly HttpClient _httpClient;
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        private bool _isProcessing = false;
        public bool IsProcessing => _isProcessing;

        public SearchService(DataContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public record SearchResult(SearchState searchState, List<LinkedInPeople> LinkedInEmployees, int totalPages);
        public async Task<SearchResult> GetSearchResult(int searchId, int page, int pageSize)
        {
            var search = await _context.Searches.FirstAsync(p => p.Id == searchId);
            var total = _context.LinkedInPeoples.Where(p => p.SearchId == searchId).ToListAsync().Result.Count();
            var employees = await _context.LinkedInPeoples.Where(p => p.SearchId == searchId)
                .OrderBy(x=>x.Id)
                .Skip((page-1)*pageSize)
                .Take(pageSize).ToListAsync();

            return new SearchResult(search.SearchState, employees, total);
        }

        public async Task<int> CreateSearch(PeopleSearchRequest peopleSearchRequest)
        {
            var search = new Search();
            search.SearchState = SearchState.Started;
            _context.Searches.Add(search);
            await _context.SaveChangesAsync();

            SearchPeopleAsync(peopleSearchRequest, search.Id);
           
            return search.Id;
        }

        public List<LinkedInPeople> linkedInPeoplesFromDb = new List<LinkedInPeople>();

        public async Task SearchPeopleAsync(PeopleSearchRequest peopleSearchRequest, int searchId)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            var search = context.Searches.First(p => p.Id == searchId);

            linkedInPeoplesFromDb = context.LinkedInPeoples.ToList();  
            
            try
            {
                var searchTechnologies = string.Join("; ", peopleSearchRequest.person_titles);
                //take first page
                var url = "https://api.apollo.io/v1/mixed_people/search";
                var request = new PeopleInternalSearchRequest(peopleSearchRequest, 1);
                
                var response = Post(request, url);

                if (response?.StatusCode == System.Net.HttpStatusCode.TooManyRequests) 
                {
                    search.StatusMessage = "Too many requests";
                    await context.SaveChangesAsync();
                    return;
                }

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response?.Content);


                // to do go thru all pages
                if (apiResponse.people != null)
                {
                    await SavePeopleToDatabaseAsync(apiResponse.people, searchId, context, searchTechnologies, linkedInPeoplesFromDb);

                    for (int i = 2; i <= apiResponse?.pagination.total_pages; i++)
                    {
                        Thread.Sleep(100);
                        var req = new PeopleInternalSearchRequest(peopleSearchRequest, i);
                        var res = Post(req, url);

                        if (res?.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                        {
                            search.StatusMessage = "Too many requests";
                            await context.SaveChangesAsync();
                            return;
                        }

                        var apiRes = JsonConvert.DeserializeObject<ApiResponse>(res?.Content);

                        await SavePeopleToDatabaseAsync(apiRes.people, searchId, context, searchTechnologies, linkedInPeoplesFromDb);
                    }
                }
                
                search.SearchState = SearchState.Done;
                search.TotalRecords = context.LinkedInPeoples.Count(x => x.SearchId == search.Id);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                search.SearchState = SearchState.Failed;
                context.SaveChanges();
            }          
        }

        public async Task SavePeopleToDatabaseAsync(List<Person> people, int searchId, DataContext context, string searchTechnologies, List<LinkedInPeople> peopleFromDB)
        {
            var contextGet = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            if (peopleFromDB.Count > 0)
            {
                var excludedDuplicates = people
                    .Where(x => !peopleFromDB.Any(p => p.LinkedInUrl == x.linkedin_url))
                    .ToList();

                SaveExtractedFilteredPeople(searchId, context, searchTechnologies, excludedDuplicates);
                await context.SaveChangesAsync();
                linkedInPeoplesFromDb.Clear();
                linkedInPeoplesFromDb = contextGet.LinkedInPeoples.ToList();
            }
            else
            {
                SaveExtractedFilteredPeople(searchId, context, searchTechnologies, people);
                await context.SaveChangesAsync();
                linkedInPeoplesFromDb.Clear();
                linkedInPeoplesFromDb = contextGet.LinkedInPeoples.ToList();
            }
        }

        private void SaveExtractedFilteredPeople(int searchId, DataContext context, string searchTechnologies, List<Person> people)
        {
            foreach (var p in people)
            {
                LinkedInPeople employee = new LinkedInPeople();

                employee.FirstName = p.first_name;
                employee.LastName = p.first_name;
                employee.Name = p.name ?? "N/A";
                employee.State = p.state ?? "N/A";
                employee.City = p.city ?? "N/A";
                employee.Country = p.country ?? "N/A";
                employee.Ranking = CalculateExperienceRanking(p.employment_history);
                employee.LinkedInUrl = p.linkedin_url;
                employee.PhoneNumber = p.phone_numbers?.FirstOrDefault()?.sanitized_number;
                employee.PhoneNumberType = p.phone_numbers?.FirstOrDefault()?.type;
                employee.Headline = p.headline ?? "N/A";
                employee.Seniority = p.seniority ?? "N/A";
                employee.Title = p.title ?? "N/A";
                employee.SearchTechnologies = searchTechnologies;
                //navigation property
                employee.SearchId = searchId;

                context.LinkedInPeoples.Add(employee);
            }
        }

        //employment history experience - ranking
        public int CalculateExperienceRanking(List<EmploymentHistory> employmentHistory)
        {
            int totalExperience = 0;
            var firstItem = employmentHistory?.FirstOrDefault()?.start_date?.Year;
            var lastItem = employmentHistory?.LastOrDefault()?.start_date?.Year;

            totalExperience = (firstItem != null ? firstItem - lastItem : DateTime.Now.Year - lastItem) ?? 2;


            if (totalExperience > 1 && totalExperience <= 2)
                return 1;
            else if (totalExperience > 2 && totalExperience <= 4)
                return 3;
            else if (totalExperience > 4 && totalExperience <= 5)
                return 5;
            else if (totalExperience > 5 && totalExperience <= 6)
                return 6;
            else if (totalExperience > 7)
                return 7;
            else
                return 1;
        }

        /// <summary>
        /// Utility function for POST REST request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IRestResponse? Get(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Accept", "application/json");

            var response = client.Execute(request);

            return response;
        }


        /// <summary>
        /// Utility function for POST REST request
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        IRestResponse? Post(object data, string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody(data);

            var response = client.Execute(request);

            return response;
        }        
    }
}
