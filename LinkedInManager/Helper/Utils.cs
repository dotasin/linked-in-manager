using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace LinkedInManager.Helper
{
    public static class Utils
    {
        public static bool IsValidEmailDomain(string email)
        {
            try
            {
                // Check if the input string is in a valid email address format
                if (!Regex.IsMatch(email, @"^(?!.*\.\@|\.@|\.\w)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    return false;
                }

                var domain = new MailAddress(email);
                // Perform domain validation here, for example, by checking DNS records
                // You can use libraries like MimeKit or MailKit for more advanced validation
                // For simplicity, let's assume any domain is valid for now
                return true;
            }
            catch
            {
                return false;
            }
        }

        public class ScientificNotationConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                // Check if the text is empty
                if (string.IsNullOrWhiteSpace(text))
                {
                    // Return null for empty strings
                    return null;
                }

                // Check if the text is in scientific notation
                if (decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value))
                {
                    // Convert the scientific notation to decimal
                    return Convert.ToDecimal(value);
                }
                else
                {
                    // Fallback to the default conversion
                    return base.ConvertFromString(text, row, memberMapData);
                }
            }
        }
    }
}
