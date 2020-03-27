using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Domain.Models.ViewModel
{
    public class 
    QuestionDetailsViewModel : MarketingSummaryBase
    {
        public QuestionDetailsViewModel()
        {
            Subjects = new List<Subject>();
            Answers = new List<Answer>();
            PaymentDetails = new List<QuestionPaymentDetail>();
            CanTheQuestionBeUpdated = false;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }
        public string CommentsUrl { get; set; }
        public string DescriptionUrl { get; set; }
        public string QuestionUrl
        {
            get { return Urls.QUESTION_URL + Id.ToString(); }
        }

        public string FACEBOOK_URL
        {
            get
            {
                return string.Format(Urls.SHARE_FACEBOOK_URL, HttpUtility.UrlEncode(QuestionUrl),
                    HttpUtility.UrlEncode(Title + " ($" + Amount + ")"));
            }
        }

        public string TWITTER_URL
        {
            get
            {
                return string.Format(Urls.SHARE_TWITTER_URL, HttpUtility.UrlEncode(QuestionUrl),
                    HttpUtility.UrlEncode(Title + " ($" + Amount + ")"));
            }
        }

        public string LINKEDIN_URL
        {
            get
            {
                var param = new object[4] 
                { 
                    HttpUtility.UrlEncode(QuestionUrl), 
                    HttpUtility.UrlEncode(Title + " ($" + Amount + ")"),
                    HttpUtility.UrlEncode(""),
                    HttpUtility.UrlEncode(QuestionUrl)
                };

                return string.Format(Urls.SHARE_LINKEDIN_URL, param);
            }
        }

        public string GOOGLE_URL
        {
            get
            {
                return string.Format(Urls.SHARE_GOOGLE_URL, HttpUtility.UrlEncode(QuestionUrl));
            }
        }


        public string Comments { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        public int StatusId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public long MarketingCampaignId { get; set; }

        [UIHint("tinymce_classic")]
        [AllowHtml]
        public string NewPostedAnswer { get; set; }

        public string EmailAddressOfUserWhoPostedQuestion { get; set; }
        public bool IsQuestionOpenForAnswers { get; set; }
        public bool CanTheQuestionBeUpdated { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public virtual Status Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionPaymentDetail> PaymentDetails { get; set; }
    }
}
