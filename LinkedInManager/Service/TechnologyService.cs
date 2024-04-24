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

        public record TechnologyResult(string message, List<Technology> technologies = null);
        public async Task<TechnologyResult> ImportTechnologies(IFormFile file)
        {
            var context = DataContext.NewDataContext(_appSettings.DbSettings.GetSqlConnectionString());
            var technologiesFromDb = await context.Technologies.ToListAsync();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string json = await reader.ReadToEndAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var techologies = JsonConvert.DeserializeObject<RootObject>(json).Tags.Where(x => x.cleaned_name != null).ToList();


                var excludedDuplicates = techologies.Where(x => !technologiesFromDb.Any(p => (p.CleanedName + p.Uid) == (x.cleaned_name + x.uid))).ToList();

                if (excludedDuplicates.Count == 0)
                {
                    return new TechnologyResult("There are no technologies to be imported - excluded by duplicate engine [p.CleanedName + p.Uid == x.CleanedName + x.Uid]", null);
                }
                else
                {
                    var list = excludedDuplicates.Select(x => new Technology()
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

                    var technologies = await context.Technologies.ToListAsync();

                    return new TechnologyResult("Technologies successfully imported!", technologies);
                }
            }
        }
    }
}