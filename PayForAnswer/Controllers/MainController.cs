using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models;
using Domain.Models.ViewModel;
using PagedList;
using Repository.Blob;
using Repository.SQL;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers
{
    public class MainController : BootstrapBaseController
    {
        public ActionResult Index(int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleAllOpenQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetAllOpenQuestions();
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        public ActionResult AllOpenQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleAllOpenQuestions;
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
            var questions = new QuestionRepository().GetUserQuestions(WebSecurity.CurrentUserId);
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        public ActionResult MyOpenQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.lblMyOpenQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetUserOpenQuestions(WebSecurity.CurrentUserId);
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        [Authorize]
        public ActionResult MyAcceptedQuestions(int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleMyAcceptedQuestions;
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            var questions = new QuestionRepository().GetUserAcceptedQuestions(WebSecurity.CurrentUserId);
            int pageNumber = (page ?? 1);
            return View("../Main/Index", questions.ToPagedList(pageNumber, Size.QuestionPagerPageSize));
        }

        /// <summary>
        /// Removes subjects
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult BySubject(int? subjectId, int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleMyAcceptedQuestions;
            QuestionsBySubjectModel questionsBySubjectModel = new SubjectBR().RemoveSubjectAndGetUserSubjectsAndQuestionsRelatedToSubjects(WebSecurity.CurrentUserId, new QuestionSubjectRepository(), subjectId);
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);
            return View(questionsBySubjectModel);
        }

        /// <summary>
        /// Adds new subjects
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        //[HttpPost]
        [Authorize]
        public ActionResult Subjects(string searchTerm = null, int? page = 1)
        {
            ViewBag.Title = CommonResources.TitleMyAcceptedQuestions;
            QuestionsBySubjectModel questionsBySubjectModel = new SubjectBR().AddSubjectAndGetUserSubjectsAndQuestionsRelatedToSubjects(WebSecurity.CurrentUserId, new QuestionSubjectRepository(), searchTerm);
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            questionsBySubjectModel.PageNumber = (page ?? 1);

            if (Request.IsAjaxRequest())
            {
                //return PartialView("_BySubject", questionsBySubjectModel);
                return PartialView("~/Views/Main/_BySubject", questionsBySubjectModel);
            }
            return View("BySubject", questionsBySubjectModel);
        }

        public ActionResult Details(Guid id, int? page = 1)
        {
            QuestionDetailsViewModel quesstionDetailsModel =
                new QuestionBR().GetQuestionDetailsModel(id, new QuestionRepository(), new BlobRepository(), DateTime.UtcNow, WebSecurity.CurrentUserId);
            ViewBag.MethodName = MethodBase.GetCurrentMethod().Name;
            int pageNumber = (page ?? 1);
            ViewBag.Answers = quesstionDetailsModel.Answers
                .OrderBy(a => a.StatusId)
                .OrderByDescending(a => a.CreatedOn)
                .ToPagedList(pageNumber, Size.AnswerPagerPageSize);
            return View(quesstionDetailsModel);
        }
    }
}