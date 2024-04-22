using LinkedInManager.Data;
using LinkedInManager.Entities;
using LinkedInManager.Models;
using LinkedInManager.Settings;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace LinkedInManager.Service
{
    public class TechnologyService : ITechnologyService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        public TechnologyService(DataContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public List<Technology> GetAllTechnologies() =>
            _context.Technologies.ToList();

        public async Task<List<Technology>> ImportTechnologies(IFormFile file)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var techologies = JsonConvert.DeserializeObject<RootObject>(json).Tags.Where(x => x.cleaned_name != null).ToList();


                var list = techologies.Select(x => new Technology()
                {
                    ApoloId = x.Id,
                    CleanedName = x.cleaned_name ?? "N/A",
                    TagNameUnanalyzedDowncase = x.tag_name_unanalyzed_downcase ?? "N/A",
                    ParentTagId = x.parent_tag_id ?? "N/A",
                    Uid = x.uid ?? "N/A",
                    Kind = x.kind ?? "N/A",
                    HasChildren = x.has_children,
                    Category = x.category ?? "N/A",
                    TagCategoryDowncase = x.tag_category_downcase ?? "N/A",
                    NumOrganizations = x.num_organizations,
                    NumPeople = x.num_people
                });


                context.Technologies.AddRange(list);
                await context.SaveChangesAsync();

                return await context.Technologies.ToListAsync();
            }
        }
    }
}
