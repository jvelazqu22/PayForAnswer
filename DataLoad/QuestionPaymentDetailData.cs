using Domain.Constants;
using Domain.Models.Entities;
using Repository.SQL;
using System;
using System.Collections.Generic;

namespace DataLoad
{
    public class QuestionPaymentDetailData
    {
        public List<QuestionPaymentDetail> GetTestDataToBeAdded(PfaDb context)
        {
            var questionPaymentDetails = new List<QuestionPaymentDetail>
                {
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 21 },
                        QuestionAmountBeforeIncrease = 10, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//1
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 21 },
                        QuestionAmountBeforeIncrease = 10, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//2
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 21 },
                        QuestionAmountBeforeIncrease = 10, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//3
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 21 },
                        QuestionAmountBeforeIncrease = 10, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//4
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 21 },
                        QuestionAmountBeforeIncrease = 10, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignReadyToBeStarted,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//5
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 18 },
                        QuestionAmountBeforeIncrease = 6, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            StartDate = DateTime.UtcNow.Subtract(new TimeSpan(10,0,0,0)),
                            EndDate = DateTime.UtcNow,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//6
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 19 },
                        QuestionAmountBeforeIncrease = 7, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            StartDate = DateTime.UtcNow.Subtract(new TimeSpan(9,0,0,0)),
                            EndDate = DateTime.UtcNow,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    },//7
                    new QuestionPaymentDetail
                    {
                        Type = QuestionPaymentDetailType.FirstPayment,
                        Payment = new Payment() { StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 20 },
                        QuestionAmountBeforeIncrease = 8, Fee = 1, TotalMarketingBudget = 10, QuestionAmountIncrease = 0,
                        CreatedOn = DateTime.UtcNow, CreatedBy = Role.PayForAnswer, UpdatedOn = DateTime.UtcNow,
                        UpdatedBy = Role.PayForAnswer, 
                        MarketingCampaign = new MarketingCampaign()
                        {
                            PerDayBudget = 1, NumberOfDaysToRun = 10, StatusId = CampaignStatus.CampaignStarted,
                            StartDate = DateTime.UtcNow.Subtract(new TimeSpan(8,0,0,0)),
                            EndDate = DateTime.UtcNow,
                            CreatedBy = Role.PayForAnswer, CreatedOn = DateTime.UtcNow, UpdatedBy = Role.PayForAnswer,
                            UpdatedOn = DateTime.UtcNow,
                        }
                    }//8
                };
            return questionPaymentDetails;
        }
    }
}
