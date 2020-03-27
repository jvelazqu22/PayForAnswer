using System.Web.Mvc;
using log4net;
using System;
using Domain.Constants;

namespace PayForAnswer.Controllers
{
    public class HelpController : BootstrapBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult ToS()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Tour()
        {
            return View();
        }

        public ActionResult Basics()
        {
            return View();
        }
    }
}
