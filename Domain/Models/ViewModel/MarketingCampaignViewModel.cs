using Domain.Constants;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.ViewModel
{
    public class MarketingCampaignViewModel
    {
        public MarketingCampaignViewModel()
        {
            SearchKeywords = new List<SearchKeyword>();
        }

        public long QuestionPaymentDetailID { get; set; }
        public string QuestionTitle { get; set; }
        public Decimal PerDayBudget { get; set; }
        public int NumberOfDaysToRun { get; set; }
        public Decimal TotalMarketingBudget { get; set; }
        public decimal Amount { get; set; }
        public decimal QuestionAmountIncrease { get; set; }
        public decimal QuestionAmountBeforeIncrease { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateString { get; set; }
        public DateTime? EndDate { get; set; }
        public string EndDateString { get; set; }
        public string CampaignManagerUserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnString { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public Guid QuestionID { get; set; }
        public string QuestionPosterUserName { get; set; }
        public string QuestionPosterEmailAddress { get; set; }
        public List<string> QuestionSubjects { get; set; }
        public string CMUserName_QuestionID_QuestionMarketingID
        {
            get 
            {
                string username = string.IsNullOrWhiteSpace(CampaignManagerUserName) ? "[username]" : CampaignManagerUserName;
                return username + "_" + QuestionID.ToString() + "_" + QuestionPaymentDetailID.ToString(); 
            }
        }
        public string Headline
        {
            get { return "$" + ((int)Amount).ToString() + " for an answer"; }
        }

        public string DisplayUrl
        {
            get { return "www.payforanswer.com"; }
        }

        public string QuestionUrl
        {
            get { return Urls.QUESTION_URL + QuestionID.ToString(); }
        }

        public List<SearchKeyword> SearchKeywords { get; set; }

        public int CampaignManagerId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
