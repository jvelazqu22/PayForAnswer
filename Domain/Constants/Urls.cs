using Domain.App_GlobalResources;
using System;
using System.Configuration;
using System.Web;

namespace Domain.Constants
{
    public static class Urls
    {

        public static string MainUrl
        {
            get { return Convert.ToString(ConfigurationManager.AppSettings["MainUrl"]); }
        }

        /// <summary>
        /// Confirm registration urls
        /// </summary>
        public static string CONFIRM_REGISTRATION = MainUrl + "/Registration/ConfirmRegistration/";

        /// <summary>
        /// Confirm email change ulrs
        /// </summary>
        public static string CONFIRM_EMAIL_CHANGE = MainUrl + "/Settings/ConfirmEmail/";

        /// <summary>
        /// Confirm pwd reset urls
        /// </summary>
        public static string CONFIRM_PWD_RESET = MainUrl + "/Settings/ResetPwd/";

        /// <summary>
        /// Question urls to be used on emial notifications. Still need to append the quesiton number.
        /// </summary>
        public static string QUESTION_URL = MainUrl + "/Main/Details/";

        public static string CAMPAIGNS_URL = MainUrl + "/MarketingCampaign/NotStartedList/";

        public static string E_NOTIFICATIONS_URL = MainUrl + "/Settings/ENotifications/";

        public static string MAIN_SHARE_FACEBOOK_URL
        {
            get
            {
                return string.Format(SHARE_FACEBOOK_URL, HttpUtility.UrlEncode(MainUrl),
                    HttpUtility.UrlEncode(CommonResources.ViewHomeIndexSubHeader3Text));
            }
        }

        public static string MAIN_SHARE_TWITTER_URL
        {
            get
            {
                return string.Format(SHARE_TWITTER_URL, HttpUtility.UrlEncode(MainUrl),
                    HttpUtility.UrlEncode(CommonResources.ViewHomeIndexSubHeader3));
            }
        }

        public static string MAIN_SHARE_LINKEDIN_URL
        {
            get
            {
                var param = new object[4] 
                { 
                    HttpUtility.UrlEncode(MainUrl), 
                    HttpUtility.UrlEncode(CommonResources.ViewHomeIndexSubHeader3),
                    HttpUtility.UrlEncode(CommonResources.ViewHomeIndexSubHeader3Text),
                    HttpUtility.UrlEncode(MainUrl)
                };

                return string.Format(SHARE_LINKEDIN_URL, param);
            }
        }

        public static string MAIN_SHAREE_GOOGLE_URL
        {
            get
            {
                return string.Format(SHARE_GOOGLE_URL, HttpUtility.UrlEncode(MainUrl));
            }
        }

        public static string FACEBOOK_PAGE_URL = "https://www.facebook.com/profile.php?id=100009254653035&fref=ts";
        public static string FACEBOOK_BIZ_PAGE_URL = "https://www.facebook.com/pages/Payforanswer/940790932621751";
        public static string TWITTER_PAGE_URL = "https://twitter.com/payforanswer";
        public static string LINKEDIN_PAGE_URL = "https://www.linkedin.com/in/payforanswer";
        public static string GOOGLE_PAGE_URL = "https://plus.google.com/u/1/106769689933565897926/posts";

        public static string SHARE_FACEBOOK_URL = "https://www.facebook.com/sharer/sharer.php?u={0}&t={1}";
        public static string SHARE_TWITTER_URL = "http://twitter.com/share?url={0}&text={1}";
        public static string SHARE_LINKEDIN_URL = "http://www.linkedin.com/shareArticle?url={0}&title={1}&summary={2}&source={3}";
        public static string SHARE_GOOGLE_URL = "https://plus.google.com/share?url={0}";
    }
}
