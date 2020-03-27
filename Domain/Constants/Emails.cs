
using System.Configuration;
namespace Domain.Constants
{
    public class Emails
    {
        public const string SUPPORT_EMAIL_ADDRESS = "support@payforanswer.com";
        public static string ReportErrorsEmailAddress
        {
            get { return ConfigurationManager.AppSettings["ReportErrorsEmailAddress"]; }
        }

    }

    public class Addressess
    {
        public const string MAIN_ADDRESS = "8622 S. 47th ave. Laveen, AZ 85339";
    }
}
