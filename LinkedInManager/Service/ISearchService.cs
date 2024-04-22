using LinkedInManager.Models;
using static LinkedInManager.Service.SearchService;

namespace LinkedInManager.Service
{
    public interface ISearchService
    {
        Task<int> CreateSearch(PeopleSearchRequest peopleSearchRequest);

        Task<SearchResult> GetSearchResult(int searchId, int page, int pageSize);
    }
}
