using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using ErrorChecking;
using log4net;
using Microsoft.AspNet.Identity;
using Repository.Blob;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class NewQuestionController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Main/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Main/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateQuestionViewModel questionViewModel)
        {
            Error error = new QuestionErrorCheckingBR().CanTheQuestionBeCreated(questionViewModel);
            if (error.ErrorFound)
            {
                Danger(error.Message, true);
                return View(questionViewModel);
            }

            questionViewModel.UserId = User.Identity.GetUserId<int>();
            questionViewModel.UserName = User.Identity.GetUserName();
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

        [Authorize]
        public ActionResult PreValidation(long id)
        {
            QuestionPaymentDetail paymentDetailModel;
            using (IPaymentRepository paymentRepository = new PaymentRepository())
                paymentDetailModel = paymentRepository.GetPaymentDetailByID(id);

            int currentUserId = User.Identity.GetUserId<int>();
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
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestionComment(Guid questionId, string comment)
        {
            Question questionModel;
            var blobRepository = new BlobRepository();
            using (IQuestionRepository questionRepository = new QuestionRepository())
                questionModel = new QuestionBR().AddQuestionComment(questionId, User.Identity.GetUserName(), comment, questionRepository, blobRepository);

            return Json(new { url = Url.Action("Details", "Main", new { id = questionModel.Id }) });
        }

        [Authorize]
        public ActionResult Error(string Description)
        {
            string errorMsg = string.Format(CommonResources.InvalidCharactersInTitle, Errors.SPECIAL_CHARACTERS);
            Danger(errorMsg, true);
            return RedirectToAction("Create");
        }
    }
}