using System;

namespace Domain.Interfaces
{
    public interface ICalculateFeesModel
    {
        decimal Amount { get; set; }
        decimal MarketingBudgetPerDay { get; set; }
        int NumberOfCampaignDays { get; set; }
        decimal QuestionAmountIncrease { get; set; }
        decimal QuestionAmountBeforeIncrease { get; set; }
        decimal Fee { get; set; }
        decimal TotalMarketingBudget { get; set; }
        decimal Total { get; set; }
    }
}
