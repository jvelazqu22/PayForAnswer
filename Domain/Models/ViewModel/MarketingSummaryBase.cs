using Domain.Constants;
using Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Domain.Models.ViewModel
{
    public class MarketingSummaryBase : UploadFile
    {
        public int TotalActiveCampaigns { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal MarketingBudgetLeft { get; set; }
        public string MarketingBudgetLeftString
        {
            get { return Math.Round(MarketingBudgetLeft, 2).ToString(); }
        }

        public DateTime? LongestCampaignEndDate { get; set; }

        public string LongestCampaignEndDateString { get; set; }
    }
}
