using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Models.Entities
{
    public class MarketingCampaign
    {
        public MarketingCampaign() 
        { 
            NumberOfDaysToRun = 0;
            PerDayBudget = 0;
            StartDate = null;
            EndDate = null;
            SearchKeywords = new List<SearchKeyword>();
        }

        //public long Id { get; set; }
        [Key()]
        public long QuestionPaymentDetailID { get; set; }
        public decimal PerDayBudget { get; set; }

        public int NumberOfDaysToRun { get; set; }

        public decimal? UsedBudget { get; set; }

        public int? StatusId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public int? CampaignManagerId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual QuestionPaymentDetail QuestionPaymentDetail { get; set; }
        public virtual Status Status { get; set; }

        [ForeignKey("CampaignManagerId")]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<SearchKeyword> SearchKeywords { get; set; }
    }
}
