using Domain.Constants;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Answer
    {
        public Answer()
        {
            Attachments = new List<Attachment>();
        }

        public long Id { get; set; }
        public Guid QuestionId { get; set; }
        public int? StatusId { get; set; }
        public int UserId { get; set; }
        public long? PaymentId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CommentsUrl { get; set; }
        public string DescriptionUrl { get; set; }

        [NotMapped]
        public string Comments { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public string StatusName { get; set; }
        [NotMapped]
        public string EmailAddressOfUserWhoPostedAnswer { get; set; }
        [NotMapped]
        public decimal QuestionAmount { get; set; }
        [NotMapped]
        public bool SendEmailToWinner { get; set; }
        [NotMapped]
        public string QuestionTitle { get; set; }
        [NotMapped]
        public bool DoesAnswerNeedsToBePaid { get; set; }
        [NotMapped]
        public string StatusStyle 
        { 
            get
            {
                if (StatusId == StatusValues.Accepted || StatusId == StatusValues.Paid)
                    return BootstrapStyles.BTN_SUCCESS;
                else if (StatusId == StatusValues.Reviewed)
                    return BootstrapStyles.BTN_WARNING;
                else
                    return BootstrapStyles.BTN_DEFAULT;
            }
        }

        public virtual ApplicationUser User { get; set; }
        public virtual Question Question { get; set; }
        public virtual Status Status { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
