
using System.Configuration;
namespace Domain.Constants
{
    public static class AttachmentType
    {
        public const int QUESTION_ATTACHMENT = 1;
        public const int ANSWER_ATTACHMENT = 2;
    }

    public static class StorageValues
    {
        public const string FEEDBACK_CONTAINER = "feedback";
        public const string COMMENTS_CONTAINER = "comments";
        public const string DESCRIPTION_CONTAINER = "descriptions";
        public const string ATTACHMENT_CONTAINER = "attachments";
        public const string NEW_QUESTION_NOTIFICATIONS_QUEUE = "new-question-notifications";
        //public const string STORAGE_URL_PRIMARY = "http://payforanswer.blob.core.windows.net";
        public static string STORAGE_URL_PRIMARY
        {
            get { return ConfigurationManager.AppSettings["StorageUrlPrimary"]; }
        }

        //public const string STORAGE_URL_SECONDARY = "http://payforanswer-secondary.blob.core.windows.net";
        public static string STORAGE_URL_SECONDARY
        {
            get { return ConfigurationManager.AppSettings["StorageUrlSecondary"]; }
        }
        public const string DELETED_HTML_URL = "http://payforanswer.blob.core.windows.net/web-content-files/Deleted.html";

        public const string COMMENTS_FILE_NAME = "comments.html";
        public const string DESCRIPTION_FILE_NAME = "description.html";
        public const string FEEDBACK_FILE_NAME = "feedback.html";

        /// QUESTION URL PLACE HOLDERS *********************************************************************
        // [url]/[container_name]/[question_id]/[attachment_id]/[name]
        public const string QUESTION_ATTACHMENT_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}/{4}";

        // [url]/[container_name]/[question_id]/[question_description_file_name]
        public const string QUESTION_DESCRIPTION_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}";

        // [url]/[container_name]/[question_id]/[question_comments_file_name]
        public const string QUESTION_COMMENT_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}";

        /// QUESTION PATH PLACE HOLDERS *********************************************************************
        // container-name/[question_id]/[attachment_id]/[name]
        public const string QUESTION_ATTACHMENT_PATH_PLACE_HOLDER = "{0}/{1}/{2}";

        // container_name/[question_id]/[question_description_file_name]
        public const string QUESTION_DESCRIPTION_PATH_PLACE_HOLDER = "{0}/{1}";

        // [container_name/[question_id]/[question_comments_file_name]
        public const string QUESTION_COMMENT_PATH_PLACE_HOLDER = "{0}/{1}";


        /// ANSWER URL PLACE HOLDERS *********************************************************************
        //[url]/[container_name]/[question_id]/[answer_id]/[attachment_id]/[name]
        public const string ANSWER_ATTACHMENT_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}/{4}/{5}";

        //[url]/[container_name]/[question_id]/[answer_id]/[question_description_file_name]
        public const string ANSWER_DESCRIPTION_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}/{4}";

        //[url]/[container_name]/[question_id]/[answer_id]/[question_comments_file_name]
        public const string ANSWER_COMMENT_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}/{4}";

        /// ANSWER PATH PLACE HOLDERS *********************************************************************
        // [question_id]/[answer_id]/[attachment_id]/[name]
        public const string ANSWER_ATTACHMENT_PATH_PLACE_HOLDER = "{0}/{1}/{2}/{3}";

        // [question_id]/[answer_id]/[question_description_file_name]
        public const string ANSWER_DESCRIPTION_PATH_PLACE_HOLDER = "{0}/{1}/{2}";

        // container_name/[question_id]/[answer_id]/[question_comments_file_name]
        public const string ANSWER_COMMENT_PATH_PLACE_HOLDER = "{0}/{1}/{2}";

        /// FEEDBACK URL PLACE HOLDERS *********************************************************************
        // [url]/[container_name]/[feedback_id]/[feedback_file_name]
        public const string FEEDBACK_URL_PLACE_HOLDER = "{0}/{1}/{2}/{3}";
        // [container_name/[feedback_id]/[feedback_file_name]
        public const string FEEDBACK_PATH_PLACE_HOLDER = "{0}/{1}";
    }
}
