namespace LinkedInManager.Models
{
    public record Breadcrumb(string label, string signal_field_name, string value, string display_name);

    public class DerivedParams
    {
    }

    public record EmploymentHistory(string _id, object created_at, bool current, object degree, object description, object emails, string end_date, object grade_level, object kind, object major, string organization_id, string organization_name, object raw_address, DateTime? start_date, string title, object updated_at, string id, string key);

    public record Organization(string id, string name, string website_url, object blog_url, object angellist_url, string linkedin_url, object twitter_url, object facebook_url, PrimaryPhone primary_phone, List<object> languages, object alexa_ranking, string phone, string linkedin_uid, int? founded_year, object publicly_traded_symbol, object publicly_traded_exchange, string logo_url, object crunchbase_url, string primary_domain, string sanitized_phone);

    public record Pagination(int? page, int? per_page, int? total_entries, int? total_pages);

    public record Person(string id, string first_name, string last_name, string name, string linkedin_url, string title, string email_status, string photo_url, object twitter_url, object github_url, object facebook_url, object extrapolated_email_confidence, string headline, string email, string organization_id, List<EmploymentHistory> employment_history, string state, string city, string country, Organization organization, bool is_likely_to_engage, List<string> departments, List<string> subdepartments, string seniority, List<string> functions, List<PhoneNumber> phone_numbers, object intent_strength, bool show_intent, bool revealed_for_current_team);

    public record PhoneNumber(string raw_number, string sanitized_number, string type, int? position, string status, object dnc_status, object dnc_other_info, object dialer_flags);

    public record PrimaryPhone(string number, string source, string sanitized_number);

    public record ApiResponse(List<Breadcrumb> breadcrumbs, bool partial_results_only, bool disable_eu_prospecting, int partial_results_limit, Pagination pagination, List<object> contacts, List<object> salesforce_users, List<Person> people, List<string> model_ids, int num_fetch_result, DerivedParams derived_params);
}
