using Microsoft.AspNetCore.Mvc;
using LinkedInManager.Service;
using LinkedInManager.Models;
using static LinkedInManager.Service.SearchService;
using Microsoft.AspNetCore.Cors;

namespace LinkedInManager.Controllers
{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/apolo")]
    [ApiController]
    public class EmployeeSearchController : ControllerBase
    {
        readonly ISearchService _searchService;
        readonly ILogger<CompanyEmployerController> _logger;
        public EmployeeSearchController(ISearchService searchService, ILogger<CompanyEmployerController> logger)
        {
            _searchService = searchService;
            _logger = logger;
        }

        [HttpPost("create-search")]
        public Task<int> CreateSearch([FromBody] PeopleSearchRequest request) =>
            _searchService.CreateSearch(request);


        [HttpGet("get-search-result")]
        public Task<SearchResult> CreateSearch(int searchId, int page, int pageSize) =>
            _searchService.GetSearchResult(searchId, page, pageSize);
    }
}
