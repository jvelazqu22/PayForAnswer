using log4net;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Controllers;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["ip"] = "1.1.1.1";
            log.Info("The PayForAnswer web application has started.");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    Response.Clear();
        //    Server.ClearError();

        //    if (ex is HttpRequestValidationException)
        //    {
        //        var routeData = new RouteData();
        //        routeData.Values["controller"] = "NewQuestion";
        //        routeData.Values["action"] = "Error";
        //        routeData.Values.Add("Description", ex.Message);
        //        //Response.StatusCode = 500;
        //        IController controller = new NewQuestionController();
        //        var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
        //        controller.Execute(rc);
        //    }
        //}
    }
}
