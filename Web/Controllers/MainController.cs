using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using Microsoft.AspNet.Identity;
using PagedList;
using Repository.Blob;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Utilities;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Domain.Models.Entities.Identity;

namespace Web.Controllers
{
    public class MainController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        public MainController() { }
        public MainController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index(int? page = 1)
        {
            ViewBag.Title = CommonResources.AllOpenQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetAllOpenQuestions();
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        public ActionResult AllOpenQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.AllOpenQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetAllOpenQuestions();
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        public ActionResult AllPaidOrAcceptedQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.lblAllPaidOrSelfAcceptedQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetAllPaidOrAcceptedQuestions();
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        public ActionResult MyQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.lblMyQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetUserQuestions(User.Identity.GetUserId<int>());
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        public ActionResult MyOpenQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.lblMyOpenQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetUserOpenQuestions(User.Identity.GetUserId<int>());
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        public ActionResult MyAcceptedQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleMyAcceptedQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetUserAcceptedQuestions(User.Identity.GetUserId<int>());
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSubjects(string commaDelimitedSubjects = null)
        {
            List<string> cleanList = new List<string>();
            Error error = new SubjectsErrorChecking().IsUserSubjectListValid(commaDelimitedSubjects, ref cleanList);
            if (error.ErrorFound)
                Danger(error.Message, true);
            else if(cleanList.Count > 0)
            {
                new SubjectBR().AddUpdateUserSubjects(User.Identity.GetUserId<int>(), cleanList, new SubjectRepository());
                Success(CommonResources.SuccessSubjectsMsg, true);
            }
            return Json(new { url = Url.Action("MySubjects", "Main") });
        }

        [Authorize]
        public ActionResult MySubjects(int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleMyAcceptedQuestions;
            QuestionsBySubjectModel questionsBySubjectModel = new SubjectBR().GetUserSubjectsAndQuestionsRelatedToSubjects(User.Identity.GetUserId<int>(), new QuestionSubjectRepository());
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);
            return View(questionsBySubjectModel);
        }

        public ActionResult Details(Guid id, int? page = 1)
        {
            int userID = User.Identity.GetUserId<int>();
            List<string> userRoles = userID == 0
                ? new List<string>()
                : UserManager.GetRoles<ApplicationUser, int>(userID).ToList();

            QuestionDetailsViewModel questionDetailsModel =
                new QuestionBR().GetQuestionDetailsModel(id, new QuestionRepository(), new BlobRepository(), DateTime.UtcNow, userID, userRoles);

            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            int pageNumber = (page ?? 1);
            ViewBag.Answers = questionDetailsModel.Answers
                .OrderBy(a => a.CreatedOn)
                .OrderBy(a => a.StatusId)
                .ToPagedList(pageNumber, Size.AnswerPagerPageSize);
            return View(questionDetailsModel);
        }
    }
}