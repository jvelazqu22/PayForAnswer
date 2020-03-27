using Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ValidateAddMoneyViewModel : ICalculateFeesModel
    {
        public Guid QuestionId { get; set; }
        public long QuestionPaymentDetailID { get; set; }
        public long PaymentId { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuestionAmountIncrease { get; set; }
        public decimal QuestionAmountBeforeIncrease { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Fee { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal MarketingBudgetPerDay { get; set; }

        public int NumberOfCampaignDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal TotalMarketingBudget { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Total { get; set; }

        public int IdOfUserTryingToMakeUpdate { get; set; }

        public int PaymentStatusId { get; set; }
        public int QuestionStatusId { get; set; }
        public int QuestionUserId { get; set; }
    }
}
