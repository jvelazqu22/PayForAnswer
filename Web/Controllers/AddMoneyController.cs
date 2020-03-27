using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using log4net;
using Microsoft.AspNet.Identity;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class AddMoneyController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Main/Create
        public ActionResult Create(Guid id)
        {
            AddMoneyViewModel addMoneyViewModel =
                new AddMoneyBR().GetAddMoneyViewModel(id, new QuestionRepository(), DateTime.UtcNow, User.Identity.GetUserId<int>());
            return View(addMoneyViewModel);
        }

        //
        // POST: /Main/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddMoneyViewModel addMoneyViewModel)
        {
            try
            {
                Error error = new AddMoneyErrorCheckingBR().ValidateAddMoneyViewModel(addMoneyViewModel);
                if (error.ErrorFound)
                {
                    Danger(error.Message);
                    return RedirectToAction("Create");
                }

                if (ModelState.IsValid)
                {
                    using (IQuestionRepository questionRepository = new QuestionRepository())
                    {
                        new AddMoneyBR().AddMoneyToQuestion(addMoneyViewModel, questionRepository, User.Identity.GetUserId<int>());
                    }
                    return RedirectToAction("PreValidation", new { id = addMoneyViewModel.QuestionPaymentDetailID });
                }

                return View(addMoneyViewModel);
            }
            catch (InvalidQuestionAmountException)
            {
                Decimal minimumQuestionAmount = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
                Decimal minimumMarketingBudget = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
                Danger(String.Format(CommonResources.MsgErrorInvalidQuestionAmount, minimumQuestionAmount, minimumMarketingBudget));
                return RedirectToAction("Create");
            }
        }

        // GET
        public ActionResult PreValidation(long id)
        {
            QuestionPaymentDetail questionPaymentDetail;
            using (IPaymentRepository paymentRepository = new PaymentRepository())
                questionPaymentDetail = paymentRepository.GetPaymentDetailByID(id);

            ValidateAddMoneyViewModel validateAddMoneyViewModel = new AddMoneyBR().GetValidationModel(questionPaymentDetail);
            validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate = User.Identity.GetUserId<int>();
            new AddMoneyErrorCheckingBR().ValidateIfQuestionPaymentDetailCanBePrevalidated(validateAddMoneyViewModel);

            return View(validateAddMoneyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreValidation(ValidateAddMoneyViewModel validateAddMoneyViewModel)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = User.Identity.GetUserId<int>();
                new AddMoneyErrorCheckingBR().ValidateIfQuestionPaymentDetailCanBePrevalidated(validateAddMoneyViewModel);
                using (IPaymentRepository paymentRepository = new PaymentRepository())
                {
                    var questionPaymentDetail = paymentRepository.GetPaymentDetailByID(validateAddMoneyViewModel.QuestionPaymentDetailID);
                    questionPaymentDetail.Payment.StatusId = StatusValues.WaitingForPaymentNotification;
                    paymentRepository.UpdatePayment(questionPaymentDetail.Payment);
                }

                return RedirectToAction("PostToPayPal", "PayPal",
                    new
                    {
                        item = validateAddMoneyViewModel.Title,
                        amount = validateAddMoneyViewModel.Total,
                        paymentId = validateAddMoneyViewModel.PaymentId
                    });
            }

            return View(validateAddMoneyViewModel);
        }

    }
}