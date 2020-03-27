using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using log4net;
using Repository.Blob;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using Repository.SQL;
using PagedList;
using System.Reflection;

namespace Web.Controllers
{
    public class HelpController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ActionResult Index()
        {
            return View();
        }

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

        [Authorize]
        public ActionResult ImproveSiteFeedback()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImproveSiteFeedback(SiteFeedbackViewModel model)
        {
            var sqlRepository = new FeedbackRepository();
            new FeedbackBR().AddFeedback(model, new BlobRepository(), sqlRepository);
            sqlRepository.Dispose();

            ViewBag.Title = CommonResources.Feedback;
            ViewBag.Heading = CommonResources.ThankYouForFeedback;
            ViewBag.Message = "";

            return View("Message");
        }

        [Authorize(Roles = Role.CommaSeparateCMandAdminRoles)]

        public ActionResult ReviewFeedback(int? page = 1)
        {
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name; 
            var sqlRepository = new FeedbackRepository();
            var feedbacklist = sqlRepository.GetFeedbackList();
            int pageNumber = (page ?? 1);
            return View(feedbacklist.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }
    }
}
