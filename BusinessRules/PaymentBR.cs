using AutoMapper;
using BusinessRules.Interfaces;
using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessRules
{
    public class PaymentBR
    {
        public PaymentBR()
        {
            Mapper.CreateMap<CreateQuestionViewModel, QuestionPaymentDetail>()
                .ForMember(dest => dest.QuestionAmountBeforeIncrease, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Type, opt => opt.UseValue(null));

            Mapper.CreateMap<AddMoneyViewModel, QuestionPaymentDetail>()
                .ForMember(dest => dest.Type, opt => opt.UseValue(null));
        }

        public void AddFirstPaymentToQuestionModel(CreateQuestionViewModel questionViewModel, Question questionModel)
        {
            Payment payment = new Payment() { StatusId = StatusValues.PaymentCreatedButNotSentToPayPalYet, Total = questionViewModel.Total };
            QuestionPaymentDetail paymentDetails = Mapper.Map<CreateQuestionViewModel, QuestionPaymentDetail>(questionViewModel);
            paymentDetails.Type = QuestionPaymentDetailType.FirstPayment;
            paymentDetails.QuestionAmountIncrease = 0;
            paymentDetails.QuestionAmountBeforeIncrease = questionModel.Amount;
            paymentDetails.CreatedBy = Role.PayForAnswer;
            paymentDetails.CreatedOn = DateTime.UtcNow;
            paymentDetails.UpdatedBy = Role.PayForAnswer;
            paymentDetails.UpdatedOn = DateTime.UtcNow;
            paymentDetails.Payment = payment;
            questionModel.QuestionPaymentDetails.Add(paymentDetails);
        }

        public QuestionPaymentDetail CreateQuestionPaymentDetailByAddingMoneyToAnExisitingQuestion(AddMoneyViewModel addMoneyViewModel, int questionPaymentDetailType)
        {
            Payment payment = new Payment() { StatusId = StatusValues.PaymentCreatedButNotSentToPayPalYet, Total = addMoneyViewModel.Total };
            QuestionPaymentDetail paymentDetails = Mapper.Map<AddMoneyViewModel, QuestionPaymentDetail>(addMoneyViewModel);
            paymentDetails.CreatedBy = addMoneyViewModel.UserName;
            paymentDetails.CreatedOn = DateTime.UtcNow;
            paymentDetails.UpdatedBy = addMoneyViewModel.UserName;
            paymentDetails.UpdatedOn = DateTime.UtcNow;
            paymentDetails.Payment = payment;
            paymentDetails.Type = questionPaymentDetailType;

            return paymentDetails;
        }

        public ValidateQuestionViewModel GetValidateQuestionModel(QuestionPaymentDetail paymentDetailModel)
        {
            ValidateQuestionViewModel validateQuestionModel = new ValidateQuestionViewModel()
            {
                QuestionId = paymentDetailModel.QuestionId,
                PaymentId = paymentDetailModel.PaymentId,
                Title = paymentDetailModel.Question.Title,
                Amount = paymentDetailModel.Question.Amount + paymentDetailModel.QuestionAmountIncrease,
                Fee = paymentDetailModel.Fee,
                MarketingBudgetPerDay = paymentDetailModel.MarketingCampaign.PerDayBudget,
                NumberOfCampaignDays = paymentDetailModel.MarketingCampaign.NumberOfDaysToRun,
                TotalMarketingBudget = paymentDetailModel.TotalMarketingBudget,
                Total = paymentDetailModel.Payment.Total,
            };

            return validateQuestionModel;
        }

        public void ProcessPaymentErrorAndThrowException(QuestionPaymentDetail questionPaymentDetailModel, PayPalModel payPalModel, IPaymentRepository paymentRepository, ref Error error)
        {
            if( error.ErrorType == StatusValues.PayPaylResponseInvalidPaymentStatus || error.ErrorType == StatusValues.PayPaylResponseTotalIsLessThanDBValue)
            {
                questionPaymentDetailModel.Payment.StatusId = error.ErrorType;
                questionPaymentDetailModel.Question.StatusId = error.ErrorType;
                paymentRepository.UpdatePaymentDetail(questionPaymentDetailModel);
            }
            if (error.ErrorType == StatusValues.PayPaylResponseInvalidPaymentStatus)
                throw new PayPalPaymentStatusNotCompleteException(error.Message);

            if (error.ErrorType == StatusValues.PayPaylResponseTotalIsLessThanDBValue)
                throw new RequestTotalAndPayPalGrossTotalMissmatchException(error.Message);

            if (error.ErrorType == StatusValues.PayPalResponseMissingPaymentID)
                throw new RequestIdMissingInPayPalResponseException(error.Message);

            if (error.ErrorType == StatusValues.PayPalResponsePaymentIDNotFound)
                throw new RequestNotFoundException(error.Message);
        }

        public QuestionPaymentDetail ValidateAndSaveQuestionPayment(PayPalModel payPalModel, IPaymentRepository paymentRepository, IEmailBR emailBR, ref Error error, IQueueRepository queueRepository)
        {
            QuestionPaymentDetail questionPaymentDetailModel = new PaymentErrorCheckingBR().CheckForErrorsAndProceedIfThereAreNoExceptions(payPalModel.PayPalResponse, paymentRepository, ref error);

            if(error.ErrorFound) SendErrorEmail(emailBR, error);
            if (error.ErrorFound && error.ErrorType != StatusValues.PayPalResponseTotalIsGreaterThanDBValue)
            {
                ProcessPaymentErrorAndThrowException(questionPaymentDetailModel, payPalModel, paymentRepository, ref error);
                return questionPaymentDetailModel;
            }

            // The payment confirmation can come via the PayPalReredirectUlr or via the Instant Payment Notification 
            // (IPN) whichever comes first will update the question status id. There is no need to update the 
            // status twice. if (questionModel.StatusId != QuestionStatusValues.PayPalConfirmed)
            if (new PaymentErrorCheckingBR().CanTheRequestBeMarkedAsPaymentReceivedForTheFirstTime(questionPaymentDetailModel, payPalModel))
            {
                var amount = questionPaymentDetailModel.Question.Amount;
                var amountIncrease = questionPaymentDetailModel.QuestionAmountIncrease;

                questionPaymentDetailModel.Payment.StatusId = payPalModel.RequestStatus;
                questionPaymentDetailModel.Question.StatusId = payPalModel.RequestStatus;
                if (new PaymentBR().DoesPaymentTypeHaveCampaign((int)questionPaymentDetailModel.Type))
                    questionPaymentDetailModel.MarketingCampaign.StatusId = CampaignStatus.CampaignReadyToBeStarted;

                if (new PaymentBR().DidPaymentIncreaseQuestionAmount((int)questionPaymentDetailModel.Type))
                {
                    questionPaymentDetailModel.Question.Amount = amount + amountIncrease;
                    questionPaymentDetailModel.Question.UpdatedOn = DateTime.UtcNow;
                }

                // so this object status can be read in the test project method 
                // payPalModel.QuestionStatus = QuestionStatusValues.PayPalConfirmed;
                paymentRepository.UpdatePaymentDetail(questionPaymentDetailModel);
                queueRepository.AddMessage(questionPaymentDetailModel.PaymentId.ToString());
                SendEmails(emailBR, questionPaymentDetailModel, paymentRepository);
            }
            return questionPaymentDetailModel;
        }

        private void SendErrorEmail(IEmailBR emailBR, Error error)
        {
            var subject = error.ErrorType == StatusValues.PayPalResponseTotalIsGreaterThanDBValue ? "Payment warning" : "Payment error";
            emailBR.SendEmail(Emails.ReportErrorsEmailAddress, subject, error.Message);
        }

        private void SendEmails(IEmailBR emailBR, QuestionPaymentDetail questionPaymentDetailModel, IPaymentRepository paymentRepository)
        {
            string confirmationUrl = confirmationUrl = Urls.QUESTION_URL + questionPaymentDetailModel.Question.Id.ToString();
            var confirmationLink = String.Format(Html.LINK_PLACE_HOLDER, confirmationUrl, questionPaymentDetailModel.Question.Title);

            var transactionTypeMsg = "";
            if (questionPaymentDetailModel.Type == QuestionPaymentDetailType.FirstPayment)
                transactionTypeMsg = CommonResources.QuestionPostedMsg;
            else if (questionPaymentDetailModel.Type == QuestionPaymentDetailType.NewMarketingCampaignWithNoQuestionIncreaseAmount)
                transactionTypeMsg = CommonResources.NewMarketingCampaignWithNoAmtIncreased;
            else if (questionPaymentDetailModel.Type == QuestionPaymentDetailType.NewMarketingCampaignWithQuestionIncreaseAmount)
                transactionTypeMsg = CommonResources.NewMarketingCampaignWithAmtIncreased;
            else if (questionPaymentDetailModel.Type == QuestionPaymentDetailType.IncreaseOfQuestionAmount)
                transactionTypeMsg = CommonResources.IncreasedQuestionAmt;

            var param = new object[4] { confirmationLink, transactionTypeMsg, questionPaymentDetailModel.CreatedOn, questionPaymentDetailModel.Payment.Total };
            string body = String.Format(CommonResources.EmailBodyOrderConfirmation, param);

            emailBR.SendEmail(questionPaymentDetailModel.Question.User.Email, CommonResources.EmailSubjectOrderConfirmation, body);

            if(questionPaymentDetailModel.Type != QuestionPaymentDetailType.NewMarketingCampaignWithNoQuestionIncreaseAmount)
            {
                ICollection<Subject> subjects = questionPaymentDetailModel.Question.Subjects.ToList();
                Task.Factory.StartNew(() => emailBR.SendNewQuestionToAllUsersWithRelatedSubject(questionPaymentDetailModel.Question, subjects, (int)questionPaymentDetailModel.Type));
            }

            if (questionPaymentDetailModel.MarketingCampaign != null &&
                questionPaymentDetailModel.MarketingCampaign.StatusId == CampaignStatus.CampaignReadyToBeStarted)
            {
                List<ApplicationUser> campaignManagers = paymentRepository.GetAllCampaignManagers();
                if(campaignManagers.Count > 0)
                    Task.Factory.StartNew(() => emailBR.NotifyAllCampaignManagers(campaignManagers));
            }
        }

        public void MarkFirstAttemptToPayAnswerAsFailed(Answer answerModel, IAnswerRepository answerRepository)
        {
            answerModel.Payment.StatusId = StatusValues.FirstAttemptToPayAnswerFailed;
            answerRepository.UpdateAnswer(answerModel);
        }

        public bool DoesPaymentTypeHaveCampaign(int questionPaymentDetailType)
        {
            if (questionPaymentDetailType == QuestionPaymentDetailType.IncreaseOfQuestionAmount)
                return false;

            return true;
        }

        public bool DidPaymentIncreaseQuestionAmount(int questionPaymentDetailType)
        {
            if (QuestionPaymentDetailType.IncreaseOfQuestionAmount == questionPaymentDetailType ||
                QuestionPaymentDetailType.NewMarketingCampaignWithQuestionIncreaseAmount == questionPaymentDetailType)
                return true;

            return false;
        }
    }
}
