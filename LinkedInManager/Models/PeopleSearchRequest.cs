namespace LinkedInManager.Models
{
    public class PeopleSearchRequest
    {
        public string[] person_locations { get; set; }
        public string[] person_titles { get; set; }
        public string[] person_seniorities { get; set; }
        public string[] q_organization_keyword_tags { get; set; }
    }
    
    public class PeopleInternalSearchRequest
    {
        public PeopleInternalSearchRequest(PeopleSearchRequest request, int? p = null) 
        {
            page = p ?? 1;
            per_page = 25;
            person_seniorities = request.person_seniorities;
            person_locations = request.person_locations;
            q_organization_keyword_tags = request.q_organization_keyword_tags;
        }
        public string[] q_organization_keyword_tags { get; set; }
        public string api_key { get; set; } = "36sOI2n4xZtsWQgN4jVOJw";
        //public string q_organization_domains { get; set; } = "apollo.io\ngoogle.com\nlinkedin.com";
        public int page { get; set; }
        public int per_page { get; set; } 
        public string[] person_seniorities { get; set; }
        public string[] person_locations { get; set; }

        public string finder_table_layout_id { get; set; } = "66015aed803e0e01c670ef91";
        public string finder_view_id { get; set; } = "5b8050d050a3893c382e9360";
        public string display_mode { get; set; } = "explorer_mode";
        public string[] open_factor_names { get; set; }
        public int num_fetch_result { get; set; } = 8;
        public string context { get; set; } = "people-index-page";
        public bool show_suggestions { get; set; } = false;
        public string ui_finder_random_seed { get; set; } = "iaruvmnpm";
        public long cacheKey { get; set; } = 1712600607465;
    }
}