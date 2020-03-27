using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class QuestionPaymentDetail
    {
        public QuestionPaymentDetail() {}

        public long QuestionPaymentDetailID { get; set; }

        public Guid QuestionId { get; set; }
        public long PaymentId { get; set; }
        public int? Type { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuestionAmountBeforeIncrease { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Fee { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal TotalMarketingBudget { get; set; }

        public decimal QuestionAmountIncrease { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        //TODO: delete this RowVersion
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [ForeignKey("Type")]
        public virtual Status Status { get; set; }
        public virtual Question Question { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual MarketingCampaign MarketingCampaign { get; set; }
    }
}
