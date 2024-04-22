using System.ComponentModel.DataAnnotations;

namespace LinkedInManager.Entities
{
    public class Technology
    {
        [Key]
        public int Id { get; set; }
        public string ApoloId { get; set; } = string.Empty;
        public string CleanedName { get; set; } = string.Empty;
        public string TagNameUnanalyzedDowncase { get; set; } = string.Empty;
        public string ParentTagId { get; set; } = string.Empty;
        public string Uid { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public bool HasChildren { get; set; }
        public string Category { get; set; } = string.Empty;
        public string TagCategoryDowncase { get; set; } = string.Empty;
        public int NumOrganizations { get; set; } = 0;
        public int NumPeople { get; set; } = 0;
    }
}