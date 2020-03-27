using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using ErrorChecking;
using log4net;
using Repository.Blob;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers
{

    public class NewQuestionController : BootstrapBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Main/Create
        public ActionResult Create(string subjectToRemove = null, string CommaDelimitedSubjects = null)
        {
            //if (subjectToRemove != null && CommaDelimitedSubjects != null)
            //{
            //    string[] subjects = CommaDelimitedSubjects.Split(',');
            //    CommaDelimitedSubjects = string.Join(",", subjects.Where(s => s != subjectToRemove).ToArray());
            //}
            return View();
        }

        //
        // POST: /Main/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(CreateQuestionViewModel questionViewModel, List<HttpPostedFileBase> fileUpload)
        public ActionResult Create(CreateQuestionViewModel questionViewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(questionViewModel.CommaDelimitedSubjects))
                {
                    Error(CommonResources.MsgErrorMissingSubjects);
                    return View(questionViewModel);
                }

                if (string.IsNullOrEmpty(questionViewModel.Description))
                {
                    Error(CommonResources.MsgErrorMissingDescription);
                    return View(questionViewModel);
                }
                questionViewModel.UserId = WebSecurity.CurrentUserId;
                questionViewModel.UserName = WebSecurity.CurrentUserName;
                if (ModelState.IsValid)
                {
                    long paymentDetailId;
                    using (IQuestionSubjectRepository questionSubjectRepository = new QuestionSubjectRepository())
                    {
                        Question questionModel = new QuestionBR().CreateQuestion(questionViewModel, questionSubjectRepository, new BlobRepository());
                        paymentDetailId = questionModel.QuestionPaymentDetails.FirstOrDefault().QuestionPaymentDetailID;
                    }
                    return RedirectToAction("PreValidation", new { id = paymentDetailId });
                }

                return View(questionViewModel);
            }
            catch (EmptyDescriptionException)
            {
                Error(CommonResources.MsgErrorMissingDescription);
                return RedirectToAction("Create");
            }
            catch (InvalidQuestionAmountException)
            {
                Decimal minimumQuestionAmount = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
                Decimal minimumMarketingBudget = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
                Decimal minimumMarketingDays = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingDays"]);
                Error(String.Format(CommonResources.MsgErrorInvalidQuestionAmount, minimumQuestionAmount, minimumMarketingBudget, minimumMarketingDays));
                return RedirectToAction("Create");
            }
        }

        [Authorize]
        public ActionResult PreValidation(long id)
        {
            QuestionPaymentDetail paymentDetailModel;
            using (IPaymentRepository paymentRepository = new PaymentRepository())
                paymentDetailModel = paymentRepository.GetPaymentDetailByID(id);

            int currentUserId = WebSecurity.CurrentUserId;
            new QuestionErrorCheckingBR().ValidateIfQuestionCanBePrevalidated(paymentDetailModel.Question, currentUserId);
            ValidateQuestionViewModel validateQuestionModel = new PaymentBR().GetValidateQuestionModel(paymentDetailModel);
            validateQuestionModel.IdOfUserTryingToMakeUpdate = currentUserId;

            return View(validateQuestionModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreValidation(ValidateQuestionViewModel validateQuestionModel)
        {
            if (ModelState.IsValid)
            {
                using (IQuestionRepository questionRepository = new QuestionRepository())
                    new QuestionBR().PrevalidateQuestion(validateQuestionModel, questionRepository);

                return RedirectToAction("PostToPayPal", "PayPal",
                    new
                    {
                        item = validateQuestionModel.Title,
                        amount = validateQuestionModel.Total,
                        paymentId = validateQuestionModel.PaymentId
                    });
            }

            return View(validateQuestionModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddQuestionComment(Guid questionId, string comment)
        {
            Question questionModel;
            var blobRepository = new BlobRepository();
            using (IQuestionRepository questionRepository = new QuestionRepository())
                questionModel = new QuestionBR().AddQuestionComment(questionId, WebSecurity.CurrentUserName, comment, questionRepository, blobRepository);

            return Json(new { url = Url.Action("Details", "Main", new { id = questionModel.Id }) });
        }

    }
}