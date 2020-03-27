using BusinessRules;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using Repository.Blob;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace Web.Controllers
{
    [Authorize]
    public class AnswerController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationUserManager _userManager;

        public AnswerController() {}

        public AnswerController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAnswer(NewAnswerViewModel newAnswerViewModel)
        {
            try
            {
                var user = UserManager.FindById(User.Identity.GetUserId<int>());
                using (IAnswerRepository answerRepository = new AnswerRepository())
                    new AnswerBR().SaveNewAnswerAndSendEmail(user, newAnswerViewModel, answerRepository, new EmailBR(), new BlobRepository());
            }
            catch (EmptyDescriptionException)
            {
                Danger(CommonResources.MsgErrorMissingAnswerDescription);
            }

            return RedirectToAction("Details", "Main", new { id = newAnswerViewModel.QuestionId });
        }

        // AJAX:
        [HttpPost]
        public ActionResult AnswerReply(long id)
        {
            return Json(new { url = Url.Action("Details", "Main", new { id = 1 }) });
        }

        // AJAX: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeclineAnswer(long id)
        {
            int userId = User.Identity.GetUserId<int>();
            Answer answerModel;
            using (IAnswerRepository answerRepository = new AnswerRepository())
                answerModel = new AnswerBR().UserUpdateAnswerStatus(id, userId, StatusValues.Reviewed, answerRepository);

            return Json(new { url = Url.Action("Details", "Main", new { id = answerModel.QuestionId }) });
        }

        // AJAX: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAnswerComment(long answerId, string comment)
        {
            Answer answerModel;
            using (IAnswerRepository answerRepository = new AnswerRepository())
                answerModel = new AnswerBR().AddAnswerComment(answerId, User.Identity.GetUserName(), comment, answerRepository, new BlobRepository(), new QuestionRepository());

            return Json(new { url = Url.Action("Details", "Main", new { id = answerModel.QuestionId }) });
        }

        public ActionResult MyAnswers(int? page = 1)
        {
            using (IAnswerRepository answerRepository = new AnswerRepository())
            {
                int pageSize = Size.UserAnswersPagerPageSize;
                int pageNumber = (page ?? 1);
                var answers = answerRepository.GetUserAnswers(User.Identity.GetUserId<int>());
                return View(answers.ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult MyPaidAnswers(int? page = 1)
        {
            using (IAnswerRepository answerRepository = new AnswerRepository())
            {
                int pageSize = Size.UserAnswersPagerPageSize;
                int pageNumber = (page ?? 1);
                var answers = answerRepository.GetUserPaidAnswers(User.Identity.GetUserId<int>());
                return View(answers.ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptAnswer(long id)
        {
            int userId = User.Identity.GetUserId<int>();
            Answer answerModel;

            using (IAnswerRepository answerRepository = new AnswerRepository())
            {
                // This is to be removed later and be replace with a queue that reads and process one request at a time.
                // This will not work when there is more than one web server running.
                object _lock = new object();
                lock (_lock)
                {
                    answerModel = new AnswerBR().UserUpdateAnswerStatus(id, userId, StatusValues.Accepted, answerRepository);
                }

                if (answerModel.DoesAnswerNeedsToBePaid && !string.IsNullOrWhiteSpace(answerModel.EmailAddressOfUserWhoPostedAnswer)
                    && answerModel.Id > 0 && answerModel.QuestionAmount >= General.MinimumQuestionAmount)
                    MassPay(answerModel, answerRepository);
            }
            Task.Factory.StartNew(() => new BlobBR().DeleteUnacceptedQuestionAnswersAttachments(answerModel, new AnswerRepository(), new BlobRepository()));

            return Json(new { url = Url.Action("Details", "Main", new { id = answerModel.QuestionId }) });
        }

        private void MassPay(Answer answerModel, IAnswerRepository answerRepository)
        {
            // Create request object
            MassPayRequestType request = new MassPayRequestType();
            ReceiverInfoCodeType receiverInfoType = (ReceiverInfoCodeType)
                Enum.Parse(typeof(ReceiverInfoCodeType), PayPalValues.IDENTIFY_PAYEE_BY_EMAIL_ADDRESS);

            request.ReceiverType = receiverInfoType;
            // (Optional) The subject line of the email that PayPal sends when the transaction completes. The subject line is the same for all recipients.
            request.EmailSubject = CommonResources.LblPaymentMade;

            // (Required) Details of each payment.
            // Note: A single MassPayRequest can include up to 250 MassPayItems.
            MassPayRequestItemType massPayItem = new MassPayRequestItemType();
            CurrencyCodeType currency = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), PayPalValues.US_CURRENCY_CODE);
            massPayItem.Amount = new BasicAmountType(currency, answerModel.QuestionAmount.ToString());

            // (Optional) How you identify the recipients of payments in this call to MassPay. It is one of the following values:
            // * EmailAddress; * UserID; * PhoneNumber
            massPayItem.ReceiverEmail = answerModel.EmailAddressOfUserWhoPostedAnswer;

            massPayItem.Note = string.Format(CommonResources.LblPaymentNote, answerModel.QuestionTitle);
            if (answerModel.Id != 0)
                massPayItem.UniqueId = answerModel.Id.ToString();

            request.MassPayItem.Add(massPayItem);

            // Invoke the API
            MassPayReq wrapper = new MassPayReq();
            wrapper.MassPayRequest = request;
            // Create the PayPalAPIInterfaceServiceService service object to make the API call
            PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService();
            // # API call 
            // Invoke the MassPay method in service wrapper object  
            MassPayResponseType massPayResponse = service.MassPay(wrapper);

            if (massPayResponse.Ack.Equals(AckCodeType.SUCCESS))
                new AnswerBR().MarkAnswerAsPaidAndSendEmail(answerModel.Id, answerRepository, new EmailBR());
            else
            {
                new PaymentBR().MarkFirstAttemptToPayAnswerAsFailed(answerModel, answerRepository);
                string exMsg = string.Format("MassPay error. Answer id: {0}, Request msg: {1}. Response msg {2}. API result {3}",
                    answerModel.Id, service.getLastRequest(), service.getLastResponse(), massPayResponse.Ack.ToString());
                Exception ex = new Exception(exMsg);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}