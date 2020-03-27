using Domain.Constants;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Question
    {
        public Question()
        {
            Subjects = new List<Subject>();
            Attachments = new List<Attachment>();
            Answers = new List<Answer>();
            QuestionPaymentDetails = new List<QuestionPaymentDetail>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, StringLength(Size.GoogleDescrLineMaxCharacters)]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        public int? StatusId { get; set; }

        public int UserId { get; set; }
        public string CommentsUrl { get; set; }
        public string DescriptionUrl { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
            
        public virtual Status Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<QuestionPaymentDetail> QuestionPaymentDetails { get; set; }
    }
}
