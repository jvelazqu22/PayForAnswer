using Domain.Constants;
using Domain.Models.Entities;
using Repository.SQL;
using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace DataLoad
{
    public class StatusData
    {
        public void InsertDefaultStatus(PfaDb context)
        {
            var status = new List<Status>
                    {
                        new Status { Id = StatusValues.QuestionRequested, Name = "Question created", DisplayName = "Incomplete" },
                        new Status { Id = StatusValues.ErrorUploadingFiles, Name = "ErrorUploadingFiles", DisplayName = "Error" },
                        new Status { Id = StatusValues.AnswerSubmitted, Name = "AnswerSubmitted", DisplayName = "Submitted" }, 
                        new Status { Id = StatusValues.PaymentCreatedButNotSentToPayPalYet, Name = "PaymentCreatedButNotSentToPayPalyet", DisplayName = "Incomplete" }, 
                        new Status { Id = StatusValues.FirstAttemptToPayAnswerFailed, Name = "FirstAttemptToPayAnswerFailed", DisplayName = "Error" }, 
                        new Status { Id = StatusValues.WaitingForPaymentNotification, Name = "Waiting for payment notification", DisplayName = "PayNow" }, 
                        new Status { Id = StatusValues.PayPalRedirectConfirmed, Name = "PayPalRedirectConfirmed", DisplayName = "Open" }, 
                        new Status { Id = StatusValues.PayPalIPNNotifyConfirmed, Name = "PayPalIPNNotifyConfirmed", DisplayName = "Open" }, 
                        new Status { Id = StatusValues.PayPalResponseMissingPaymentID, Name = "PayPalResponseMissingPaymentID", DisplayName = "Error" }, 
                        new Status { Id = StatusValues.PayPalResponsePaymentIDNotFound, Name = "PayPalResponsePaymentIDNotFound", DisplayName = "Error" }, 
                        new Status { Id = StatusValues.PayPaylResponseTotalIsLessThanDBValue, Name = "PayPaylResponseTotalIsLessThanDBValue", DisplayName = "Error" }, 
                        new Status { Id = StatusValues.PayPalResponseTotalIsGreaterThanDBValue, Name = "PayPalResponseTotalIsGreaterThanDBValue", DisplayName = "Warning" }, 
                        new Status { Id = StatusValues.PayPaylResponseInvalidPaymentStatus, Name = "PayPaylResponseInvalidPaymentStatus", DisplayName = "Error" }, 
                        new Status { Id = StatusValues.Accepted, Name = "Accepted", DisplayName = "Accepted" }, 
                        new Status { Id = StatusValues.AcceptedByRequester, Name = "AcceptedByRequester", DisplayName = "Self Accepted" }, 
                        new Status { Id = StatusValues.Paid, Name = "Paid", DisplayName = "Paid" }, 
                        new Status { Id = StatusValues.Reviewed, Name = "Reviewed", DisplayName = "Reviewed" }, 
                        new Status { Id = StatusValues.Error, Name = "Error", DisplayName = "Error" }, 

                        new Status { Id = CampaignStatus.CampaignReadyToBeStarted, Name = "CampaignReadyToBeStarted", DisplayName = "Campaign is ready to be started." }, 
                        new Status { Id = CampaignStatus.CampaignManagerAssigned, Name = "CampaignManagerAssiged", DisplayName = "Campaign Manager Assigned" }, 
                        new Status { Id = CampaignStatus.CampaignStarted, Name = "CampaigngStarted", DisplayName = "Campaign Started" }, 

                        new Status { Id = QuestionPaymentDetailType.FirstPayment, Name = "First Payment", DisplayName = "First Payment" }, 
                        new Status { Id = QuestionPaymentDetailType.NewMarketingCampaignWithNoQuestionIncreaseAmount, Name = "NewMarketingCampaignWithNoQuestionIncreaseAmount", DisplayName = "NewMarketingCampaignWithNoQuestionIncreaseAmount" }, 
                        new Status { Id = QuestionPaymentDetailType.NewMarketingCampaignWithQuestionIncreaseAmount, Name = "NewMarketingCampaignWithQuestionIncreaseAmount", DisplayName = "NewMarketingCampaignWithQuestionIncreaseAmount" }, 
                        new Status { Id = QuestionPaymentDetailType.IncreaseOfQuestionAmount, Name = "IncreaseOfQuestionAmount", DisplayName = "IncreaseOfQuestionAmount" }, 
                    };
            status.ForEach(s => context.Status.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
