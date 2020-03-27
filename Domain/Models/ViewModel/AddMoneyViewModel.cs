using Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.ViewModel
{
    public class AddMoneyViewModel : MarketingSummaryBase, ICalculateFeesModel
    {
        public AddMoneyViewModel() { CanUserViewSummary = false; }

        public Guid QuestionId { get; set; }
        public long QuestionPaymentDetailID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Subjects")]
        public string CommaDelimitedSubjects { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuestionAmountIncrease { get; set; }
        public decimal QuestionAmountBeforeIncrease { get; set; }

        public bool CanUserViewSummary { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal MarketingBudgetPerDay { get; set; }

        public int NumberOfCampaignDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal TotalMarketingBudget { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Fee { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Total { get; set; }
        public string UserName { get; set; }
    }
}
