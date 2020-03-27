//using Microsoft.Azure.Jobs;
using BusinessRules;
using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Microsoft.Azure.WebJobs;
using Repository.Interfaces;
using Repository.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Notifications
{
    public class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }

        public void ProcessQueueMessage([QueueTrigger(StorageValues.NEW_QUESTION_NOTIFICATIONS_QUEUE)] string paymentID, TextWriter logger)
        {
            try
            {
                logger.WriteLine(paymentID);
                //long paymentId = Convert.ToInt64(paymentID);
                //var paymentRepository = new PaymentRepository();
                //var emailBR = new EmailBR();
                //QuestionPaymentDetail paymentModel = paymentRepository.GetPaymentDetailByPaymentID(paymentId);

                //NotifyNewQuestionToAllUsersWithRelatedSubject(paymentModel, emailBR);
                //NotifyAllCampaignManagers(paymentModel, paymentRepository, emailBR);
            }
            catch (Exception ex)
            {
                string errorMsg = string.Format("Exception message: {0}, Inner Exception: {1}, Stack Trace: {2}", ex.Message, ex.InnerException, ex.StackTrace);
                logger.WriteLine(errorMsg);
            }
        }

        public void NotifyNewQuestionToAllUsersWithRelatedSubject(QuestionPaymentDetail paymentModel, IEmailBR emailBR)
        {
            if (paymentModel.Type != QuestionPaymentDetailType.NewMarketingCampaignWithNoQuestionIncreaseAmount)
            {
                ICollection<Subject> subjects = paymentModel.Question.Subjects.ToList();
                emailBR.SendNewQuestionToAllUsersWithRelatedSubject(paymentModel.Question, subjects, (int)paymentModel.Type);
            }
        }

        public void NotifyAllCampaignManagers(QuestionPaymentDetail paymentModel, IPaymentRepository paymentRepository, IEmailBR emailBR)
        {
            if (paymentModel.MarketingCampaign != null &&
                paymentModel.MarketingCampaign.StatusId == CampaignStatus.CampaignReadyToBeStarted)
            {
                List<ApplicationUser> campaignManagers = paymentRepository.GetAllCampaignManagers();
                if (campaignManagers.Count > 0)
                    emailBR.NotifyAllCampaignManagers(campaignManagers);
            }
        }
    }
}
