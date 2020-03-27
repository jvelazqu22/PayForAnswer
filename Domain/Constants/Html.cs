
namespace Domain.Constants
{
    public static class Html
    {
        public const string COMMENTS = "<tr><td><small><strong>{0} {1} : </strong> {2} </small> </td></tr>";
        public const string COMMENTS_BEGIN_TABLE = "<table class='table table-bordered table-striped'>";
        public const string COMMENTS_END_TABLE = "</table>";
        public const string COMMENTS_DEFAULT_VALUE = "<tr><td></td></tr>";
        public const string DESCRIPTION_DEFAULT_VALUE = "description";
        public const string LINK_PLACE_HOLDER = "<a href=\"{0}\">{1} </a>";
        public const string LINK_PLACE_HOLDER_NEW_WINDOW = "<a href=\"{0}\" target=\"_blank\">{1}</a>";
        public const string EMAIL_LINK_PLACE_HOLDER = "<a href=\"mailto:{0}\">{1} </a>";
        
        public const string TOP_QUESTION_AD_NEW_WINDOW_END_MARKER = "</a><br/>";
        public const string TOP_QUESTION_AD_NEW_WINDOW = "<a href=\"{0}\" target=\"_blank\">{1}" + TOP_QUESTION_AD_NEW_WINDOW_END_MARKER;
        //public const string TOP_QUESTION_AD_NEW_WINDOW = "<a href=\"{0}\" target=\"_blank\">{1}</a>" +
        //    "<br/><span class='text-success'>{2}</span><br/>{3}<br/>";
    }

    public static class BootstrapStyles
    {
        public const string BTN_DEFAULT = "btn-default";
        public const string BTN_SUCCESS = "btn-success";
        public const string BTN_DANGER = "btn-danger";
        public const string BTN_WARNING = "btn-warning";
    }
}
