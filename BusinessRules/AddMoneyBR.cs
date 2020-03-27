using Domain.Constants;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.ViewModel;
using ErrorChecking;
using Repository.Interfaces;
using System;
using System.Linq;

namespace BusinessRules
{
    public class AddMoneyBR
    {
        public AddMoneyViewModel GetAddMoneyViewModel(Guid questionId, IQuestionRepository questionRepository, DateTime utcNow, int currentUserId)
        {
            AddMoneyViewModel addMoneyViewModel = new AddMoneyViewModel();
            Question question = questionRepository.GetQuestionByID(questionId);
            new AddMoneyErrorCheckingBR().ValidateQuestionAndUser(question, currentUserId, questionId);

            addMoneyViewModel.QuestionId = questionId;
            addMoneyViewModel.Title = question.Title;
            addMoneyViewModel.CommaDelimitedSubjects = string.Join(",", question.Subjects.Select(s => s.SubjectName).ToArray());
            addMoneyViewModel.Amount = question.Amount;
            if (currentUserId == question.UserId)
            {
                new MarketingBR().GetQuestionActiveMarketingCampaignSummary(question, addMoneyViewModel, utcNow);
                addMoneyViewModel.CanUserViewSummary = true;
            }
            else
                addMoneyViewModel.CanUserViewSummary = false;

            return addMoneyViewModel;
        }

        public void AddMoneyToQuestion(AddMoneyViewModel addMoneyViewModel, IQuestionRepository questionRepository, int currentUserId)
        {
            Question questionModel = questionRepository.GetQuestionByID(addMoneyViewModel.QuestionId);
            new AddMoneyErrorCheckingBR().ValidateQuestionAndUser(questionModel, currentUserId, addMoneyViewModel.QuestionId);
            addMoneyViewModel.UserName = questionModel.User.UserName;
            int questionPaymentDetailType = GetQuestionPaymentDetailType(addMoneyViewModel);
            CalculateFees(addMoneyViewModel, questionPaymentDetailType);
            QuestionPaymentDetail paymentDetails = new PaymentBR().CreateQuestionPaymentDetailByAddingMoneyToAnExisitingQuestion(addMoneyViewModel, questionPaymentDetailType);

            if ( new PaymentBR().DoesPaymentTypeHaveCampaign((int)paymentDetails.Type) )
                paymentDetails.MarketingCampaign = new MarketingBR().CreateMarketingCampaign(addMoneyViewModel);

            questionModel.QuestionPaymentDetails.Add(paymentDetails);
            questionRepository.UpdateQuestion(questionModel);
            addMoneyViewModel.QuestionPaymentDetailID = paymentDetails.QuestionPaymentDetailID;
        }

        public int GetQuestionPaymentDetailType(AddMoneyViewModel addMoneyViewModel)
        {
            if (addMoneyViewModel.QuestionAmountIncrease > 0 && addMoneyViewModel.MarketingBudgetPerDay > 0 && addMoneyViewModel.NumberOfCampaignDays > 0)
                return QuestionPaymentDetailType.NewMarketingCampaignWithQuestionIncreaseAmount;
            else if (addMoneyViewModel.QuestionAmountIncrease == 0 && addMoneyViewModel.MarketingBudgetPerDay > 0 && addMoneyViewModel.NumberOfCampaignDays > 0)
                return QuestionPaymentDetailType.NewMarketingCampaignWithNoQuestionIncreaseAmount;
            else if (addMoneyViewModel.QuestionAmountIncrease > 0 && addMoneyViewModel.MarketingBudgetPerDay == 0 && addMoneyViewModel.NumberOfCampaignDays == 0)
                return QuestionPaymentDetailType.IncreaseOfQuestionAmount;

            return 0;
        }

        public ICalculateFeesModel CalculateFees(ICalculateFeesModel calculateFeesModel, int questionPaymentDetailType)
        {
            Decimal percentageFee = General.ChargePercentageFee;
            Decimal fixFee = General.ChargeFixFee;

            calculateFeesModel.QuestionAmountIncrease = Math.Round(calculateFeesModel.QuestionAmountIncrease, 2);
            calculateFeesModel.QuestionAmountBeforeIncrease = calculateFeesModel.Amount;
            calculateFeesModel.QuestionAmountBeforeIncrease = Math.Round(calculateFeesModel.QuestionAmountBeforeIncrease, 2);
            calculateFeesModel.MarketingBudgetPerDay = Math.Round((decimal)calculateFeesModel.MarketingBudgetPerDay, 2);
            calculateFeesModel.TotalMarketingBudget = calculateFeesModel.MarketingBudgetPerDay * calculateFeesModel.NumberOfCampaignDays;
            calculateFeesModel.Fee = ((calculateFeesModel.QuestionAmountIncrease + calculateFeesModel.MarketingBudgetPerDay) * percentageFee) + fixFee;

            calculateFeesModel.Fee = calculateFeesModel.Fee > General.MaxChargeFee 
                ? General.MaxChargeFee
                : Math.Round(Convert.ToDecimal(calculateFeesModel.Fee), 2);

            calculateFeesModel.Total = calculateFeesModel.QuestionAmountIncrease + calculateFeesModel.TotalMarketingBudget + calculateFeesModel.Fee;
            calculateFeesModel.Total = Math.Round(Convert.ToDecimal(calculateFeesModel.Total), 2);

            return calculateFeesModel;
        }

        public ValidateAddMoneyViewModel GetValidationModel(QuestionPaymentDetail questionPaymentDetail)
        {
            ValidateAddMoneyViewModel validateAddMoneyViewModel = new ValidateAddMoneyViewModel();
            validateAddMoneyViewModel.QuestionId = questionPaymentDetail.QuestionId;
            validateAddMoneyViewModel.PaymentId = questionPaymentDetail.PaymentId;
            validateAddMoneyViewModel.QuestionPaymentDetailID = questionPaymentDetail.QuestionPaymentDetailID;
            validateAddMoneyViewModel.Title = questionPaymentDetail.Question.Title;
            validateAddMoneyViewModel.Amount = questionPaymentDetail.Question.Amount;
            validateAddMoneyViewModel.QuestionAmountIncrease = questionPaymentDetail.QuestionAmountIncrease;
            validateAddMoneyViewModel.Fee = questionPaymentDetail.Fee;
            if (new PaymentBR().DoesPaymentTypeHaveCampaign((int)questionPaymentDetail.Type))
            {
                validateAddMoneyViewModel.MarketingBudgetPerDay = questionPaymentDetail.MarketingCampaign.PerDayBudget;
                validateAddMoneyViewModel.NumberOfCampaignDays = questionPaymentDetail.MarketingCampaign.NumberOfDaysToRun;
                validateAddMoneyViewModel.TotalMarketingBudget = questionPaymentDetail.TotalMarketingBudget;
            }
            validateAddMoneyViewModel.PaymentStatusId = (int)questionPaymentDetail.Payment.StatusId;
            validateAddMoneyViewModel.QuestionStatusId = (int)questionPaymentDetail.Question.StatusId;
            validateAddMoneyViewModel.QuestionUserId = (int)questionPaymentDetail.Question.UserId;

            CalculateFees(validateAddMoneyViewModel, (int)questionPaymentDetail.Type);

            return validateAddMoneyViewModel;
        }
    }
}
