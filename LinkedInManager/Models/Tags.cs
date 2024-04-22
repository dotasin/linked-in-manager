using Newtonsoft.Json;

namespace LinkedInManager.Models
{
    public class RootObject
    {
        public List<Tag> Tags { get; set; }
    }

    public class Tag
    {
        public string Id { get; set; }
        public string cleaned_name { get; set; }
        public string tag_name_unanalyzed_downcase { get; set; }
        public string parent_tag_id { get; set; }
        public string uid { get; set; }
        public string kind { get; set; }
        public bool has_children { get; set; }
        public string category { get; set; }
        public string tag_category_downcase { get; set; }
        public int num_organizations { get; set; }
        public int num_people { get; set; }

        [JsonProperty("_index_type")]
        public string IndexType { get; set; }
        [JsonProperty("_score")]
        public double Score { get; set; }
        [JsonProperty("_explanation")]
        public object Explanation { get; set; }
    }
}
