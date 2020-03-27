using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using log4net;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Configuration;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace PayForAnswer.Controllers
{

    [Authorize]
    public class AddMoneyController : BootstrapBaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Main/Create
        public ActionResult Create(Guid id)
        {
            AddMoneyViewModel addMoneyViewModel =
                new AddMoneyBR().GetAddMoneyViewModel(id, new QuestionRepository(), DateTime.UtcNow, WebSecurity.CurrentUserId);
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
                    Error(error.Message);
                    return RedirectToAction("Create");
                }

                if (ModelState.IsValid)
                {
                    using (IQuestionRepository questionRepository = new QuestionRepository())
                    {
                        new AddMoneyBR().AddMoneyToQuestion(addMoneyViewModel, questionRepository, WebSecurity.CurrentUserId);
                    }
                    return RedirectToAction("PreValidation", new { id = addMoneyViewModel.QuestionPaymentDetailID });
                }

                return View(addMoneyViewModel);
            }
            catch (InvalidQuestionAmountException)
            {
                Decimal minimumQuestionAmount = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
                Decimal minimumMarketingBudget = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
                Error(String.Format(CommonResources.MsgErrorInvalidQuestionAmount, minimumQuestionAmount, minimumMarketingBudget));
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
            validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate = WebSecurity.CurrentUserId;
            new AddMoneyErrorCheckingBR().ValidateIfQuestionPaymentDetailCanBePrevalidated(validateAddMoneyViewModel);

            return View(validateAddMoneyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreValidation(ValidateAddMoneyViewModel validateAddMoneyViewModel)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = WebSecurity.CurrentUserId;
                new AddMoneyErrorCheckingBR().ValidateIfQuestionPaymentDetailCanBePrevalidated(validateAddMoneyViewModel);

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