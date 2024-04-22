using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace LinkedInManager.Helper
{
    public static class HelperUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fiscalCode"></param>
        /// <returns></returns>
        public static string QRCodeDataSempler(string fiscalCode)
        {
            var fiscal_Data = $"fiscal_code{fiscalCode};date{DateTime.Now.ToString("ddMMyyyy hh:mm:ss")};store_id;C9W7+525;store_street;Comayagua;store_town;Honduras;zip_code;15101;store_email;beltech@office.com";

            return Base64Encode(fiscal_Data);
        }

        /// <summary>
        /// Because we don't use database for this mockup fiscal api we generate random code for fiscal_code
        /// </summary>
        /// <returns></returns>
        public static string CodeRandomizer()
        {
            int length = 7;

            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Tries to format string as Json string, if it doesn't succeed
        /// it returns original string back.
        /// </summary>
        public static string FormatJsonOrSelf(this string target)
        {
            try
            {
                return FormatJson(target);
            }
            catch
            {
                return target;
            }
        }

        public static string ExtractJsonFromHtml(string html)
        {
            // Define regular expression pattern to match JSON content
            string pattern = @"JSON.parse\('([^']+)'\)";

            // Search for matches
            Match match = Regex.Match(html, pattern);

            // Check if a match is found
            if (match.Success)
            {
                // Extract and return the JSON content
                return match.Groups[1].Value;
            }
            else
            {
                // No JSON content found
                return null;
            }
        }

        public static string FormatJson(this string content)
        {
            string jsonFormatter = string.Empty;
            if (DetermineContentType(content) == ContentType.HTML)
            {
                jsonFormatter = ExtractJsonFromHtml(content);
                content = jsonFormatter;
            }

            if (content.IsNullOrWhiteSpace())
                return "";


            // Check if JSON content is extracted successfully
            if (content == null)
            {
                // No JSON content found, return empty string
                return "";
            }

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);

            return JsonSerializer.Serialize(jsonElement, options);
        }

        public enum ContentType
        {
            Unknown,
            HTML,
            XML,
            JSON
        }
        public static ContentType DetermineContentType(string content)
        {
            // Check if content is empty
            if (string.IsNullOrWhiteSpace(content))
            {
                return ContentType.Unknown;
            }

            // Check if content starts with '<!DOCTYPE html>' or '<html>'
            if (content.Trim().StartsWith("<!DOCTYPE html>") || content.Trim().StartsWith("<!-- HTML"))
            {
                return ContentType.HTML;
            }

            // Check if content starts with '<' and ends with '>'
            if (content.Trim().StartsWith("<") && content.Trim().EndsWith(">"))
            {
                return ContentType.XML;
            }

            // Check if content starts with '{' or '['
            if (content.Trim().StartsWith("{") || content.Trim().StartsWith("["))
            {
                return ContentType.JSON;
            }

            // If none of the above conditions are met, return Unknown
            return ContentType.Unknown;
        }

        public static bool IsNullOrWhiteSpace(this string value) =>
                    string.IsNullOrEmpty(value) || value.Trim().Length == 0;

        public static IEnumerable<TItem> UnfoldSingle<TItem>(this TItem obj, Func<TItem, TItem> selector, bool includeSelf = true)
        {
            var items = new List<TItem>();

            if (includeSelf)
            {
                items.Add(obj);
            }

            var next = selector(obj);

            if (next != null)
            {
                items.AddRange(
                    UnfoldSingle<TItem>(next, selector));
            }

            return items;
        }


        public static string TruncateLongString(this string str, int maxLength, bool addElipsis = true)
        {
            var res = str.Substring(0, Math.Min(str.Length, maxLength));

            if (addElipsis && str.Length > maxLength)
                res += "...";

            return res;
        }
        public static string Join<T>(this IEnumerable<T> values, string separator) =>
           string.Join(separator, values);
    }
}
