// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeneralConstants.cs" company="">
//   
// </copyright>
// <summary>
//   The general constants.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Domain.Constants
{
    /// <summary>
    /// The general constants.
    /// </summary>
    public static class StatusValues
    {
        public const int QuestionRequested = 1;

        public const int ErrorUploadingFiles = 2;

        public const int AnswerSubmitted = 3;

        public const int PaymentCreatedButNotSentToPayPalYet = 4;

        public const int FirstAttemptToPayAnswerFailed = 5;

        public const int WaitingForPaymentNotification = 6;

        public const int PayPalRedirectConfirmed = 7;

        public const int PayPalIPNNotifyConfirmed = 8;

        public const int PayPalResponseMissingPaymentID = 9;

        public const int PayPalResponsePaymentIDNotFound = 10;

        public const int PayPaylResponseTotalIsLessThanDBValue = 11;

        public const int PayPalResponseTotalIsGreaterThanDBValue = 12;

        public const int PayPaylResponseInvalidPaymentStatus = 13;

        public const int Accepted = 14;

        /// <summary>
        /// The answer accepted was posted by the same user who posted the question
        /// </summary>
        public const int AcceptedByRequester = 15;

        /// <summary>
        /// This value is used when an answer has been paid to a user
        /// </summary>
        public const int Paid = 16;

        /// <summary>
        /// Answer status
        /// </summary>
        public const int Reviewed = 17;
        
        public const int Error = 18;

        /// <summary>
        /// Payment status value return by paypal and trasanction was successfully completed
        /// </summary>
        public const string PayPalCompletedPaymentStatus = "Completed";
    }


    public static class CampaignStatus
    {
        public const int CampaignReadyToBeStarted = 101;
        public const int CampaignManagerAssigned = 102;
        public const int CampaignStarted = 103;
    }

    public static class QuestionPaymentDetailType
    {
        public const int FirstPayment = 200;
        public const int NewMarketingCampaignWithNoQuestionIncreaseAmount = 201;
        public const int NewMarketingCampaignWithQuestionIncreaseAmount = 202;
        public const int IncreaseOfQuestionAmount = 203;
    }
    /// <summary>
    /// 
    /// </summary>
    public class StatusList
    {
        static public List<int> CONFIRM_PAYMENT_STATUS = new List<int>(
            new int[]
                {
                    StatusValues.PayPalIPNNotifyConfirmed,
                    StatusValues.PayPalRedirectConfirmed
                });

        static public List<int> CAMPAIGN_STATUS = new List<int>(
            new int[]
                {
                    CampaignStatus.CampaignReadyToBeStarted,
                    CampaignStatus.CampaignManagerAssigned,
                    CampaignStatus.CampaignStarted
                });

        static public List<int> CAMPAIGN_STARTED_AND_MANAGER_ASSIGEND_STATUS = new List<int>(
            new int[]
                {
                    CampaignStatus.CampaignReadyToBeStarted,
                    CampaignStatus.CampaignManagerAssigned,
                });
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }

}
