using System;

namespace Domain.Models.ViewModel
{
    public class QuestionMarketingHistoryViewModel
    {
        public Guid QuestionID { get; set; }
        public string QuestionTitle { get; set; }
        public Decimal PerDayBudget { get; set; }
        public int NumberOfDaysToRun { get; set; }
        public Decimal TotalMarketingBudget { get; set; }
        public decimal QuestionAmountIncrease { get; set; }
        public decimal QuestionAmountBeforeIncrease { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime EndDate { get; set; }
        public string EndDateString { get; set; }
        public long QuestionPaymentDetailID { get; set; }
        public DateTime CratedOn { get; set; }
        public string CreatedOnString { get; set; }
        public string Status { get; set; }
    }
}
