using System.Web.Optimization;

namespace PayForAnswer.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/PayPal/js").Include(
                "~/Scripts/jquery-{version}.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                "~/Scripts/jquery-{version}.min.js",
                "~/Scripts/jquery-migrate-{version}.min.js",
                "~/Scripts/jquery-ui-{version}.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/jquery.validate-vsdoc.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/typeahead.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/application.js",
                "~/Scripts/Pfa.js"
            ));

            //bundles.Add(new ScriptBundle("~/Scripts/tinymce")
            //            .Include("~/Scripts/tinymce/tinymce.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                "~/Content/bootstrap/bootstrap.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/typeahead.js-bootstrap.css",
                "~/Content/bootstrap-overwrite.css",
                "~/Content/bootstrap-mvc-validation.css"
            ));
            //.Include("~/Content/site.css")
            //.Include("~/Content/PayForAnswer.css")
            //.Include("~/Content/PagedList.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.IgnoreList.Clear();
        }
    }
}