using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Domain.Models.ViewModel;
using System;

namespace ErrorChecking
{
    public class AddMoneyErrorCheckingBR
    {
        public Error ValidateAddMoneyViewModel(AddMoneyViewModel addMoneyViewModel)
        {
            string errorMsg = string.Empty;
            if (addMoneyViewModel.QuestionAmountIncrease > 0 && addMoneyViewModel.QuestionAmountIncrease < General.MinimumQuestionAmountIncrease)
            {
                errorMsg = string.Format(CommonResources.InvalidQuestionAmtIncreaseErrorMsg, General.MinimumQuestionAmountIncrease);
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.MIN_INCREASE_Q_AMT_NOT_MET, Message = errorMsg }; 
            }
            if (addMoneyViewModel.MarketingBudgetPerDay > 0 && addMoneyViewModel.MarketingBudgetPerDay < General.MinimumMarketingBudgetPerDay)
            {
                errorMsg = string.Format(CommonResources.InvalidMarketingBudgetPerDayErrorMsg, General.MinimumMarketingBudgetPerDay);
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.MIN_MARKETING_BUDGET_PER_DAY_NOT_MET, Message = errorMsg }; 
            }
            if (addMoneyViewModel.NumberOfCampaignDays > 0 && addMoneyViewModel.NumberOfCampaignDays < General.MinimumMarketingDays)
            {
                errorMsg = string.Format(CommonResources.InvalidMarketingCampaignDaysErrorMsg, General.MinimumMarketingDays);
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.MIN_MARKETING_DAYS_NOT_MET, Message = errorMsg }; 
            }
            if (addMoneyViewModel.MarketingBudgetPerDay == 0 && addMoneyViewModel.NumberOfCampaignDays > 0)
            {
                errorMsg = CommonResources.MarketingBudgetPerDayZeroAndCampaignDaysNotZeroErrorMsg;
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.MISSING_MARKETING_BUDGET_PER_DAY, Message = errorMsg }; 
            }
            if (addMoneyViewModel.MarketingBudgetPerDay > 0 && addMoneyViewModel.NumberOfCampaignDays == 0)
            {
                errorMsg = CommonResources.MarketingBudgetPerDayNotZeroAndCampaignDaysZeroErrorMsg;
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.MISSING_NUMBER_OF_MARKETING_CAMPAIGN_DAYS, Message = errorMsg };
            }
            if (addMoneyViewModel.QuestionAmountIncrease == 0 && addMoneyViewModel.MarketingBudgetPerDay == 0 && addMoneyViewModel.NumberOfCampaignDays == 0)
            {
                errorMsg = CommonResources.AddMoneyEmptyValuesErrorMsg;
                return new Error() { ErrorFound = true, ErrorType = AddingMoneyErrors.NO_VALUES_ENTERED, Message = errorMsg };
            }
            return new Error() { ErrorFound = false };
        }

        public void ValidateQuestionAndUser(Question questionModel, int currentUserId, Guid questionId)
        {
            if (questionModel == null) throw new RequestNotFoundException(string.Format("Question id: {0}", questionId));

            if (questionModel == null)
                throw new Exception("questionModel == null");

            if (questionModel.UserId != currentUserId)
                throw new InvalidUserException("Question.UserId: " + questionModel.UserId + " currentUserId: " + currentUserId);
        }

        public void ValidateIfQuestionPaymentDetailCanBePrevalidated(ValidateAddMoneyViewModel validateAddMoneyViewModel)
        {
            string details = "QuestionPaymentDetail id: " + validateAddMoneyViewModel.QuestionPaymentDetailID.ToString() + " - Payment status: " + validateAddMoneyViewModel.PaymentStatusId +
                " Question status: " + validateAddMoneyViewModel.QuestionStatusId + " - Question user id: " + validateAddMoneyViewModel.QuestionUserId + " - Id of user trying to make update: " +
                validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate;
            if (StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)validateAddMoneyViewModel.PaymentStatusId) || validateAddMoneyViewModel.QuestionStatusId == StatusValues.Paid
                || validateAddMoneyViewModel.QuestionStatusId == StatusValues.Accepted || validateAddMoneyViewModel.QuestionStatusId == StatusValues.AcceptedByRequester)
                throw new AddMoneyPrevalidationException(details);

            if (validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate != validateAddMoneyViewModel.QuestionUserId)
                throw new AddMoneyPrevalidationException(details);
        }
    }
}
