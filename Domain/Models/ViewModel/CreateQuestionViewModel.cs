using Domain.Constants;
using Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Domain.Models
{

    public class CreateQuestionViewModel : UploadFile
    {
        public CreateQuestionViewModel(){}

        [Required, StringLength(Size.GoogleDescrLineMaxCharacters)]
        public string Title { get; set; }

        [AllowHtml]
        [Required]
        public string Description { get; set; }

        [AllowHtml]
        [Required]
        public string GoogleSearchKeywords1 { get; set; }

        [AllowHtml]
        [Required]
        public string GoogleSearchKeywords2 { get; set; }

        [AllowHtml]
        [Required]
        public string GoogleSearchKeywords3 { get; set; }

        [AllowHtml]
        [Required]
        public string GoogleSearchKeywords4 { get; set; }

        [AllowHtml]
        [Required]
        public string GoogleSearchKeywords5 { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")] 
        public decimal Fee { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal MarketingBudgetPerDay { get; set; }

        public int NumberOfCampaignDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal TotalMarketingBudget { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Total { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public long MarketingCampaignId { get; set; }

        [UIHint("tinymce_classic")]
        [AllowHtml]
        public string NewPostedAnswer { get; set; }

        [Display(Name = "Subjects")]
        public string CommaDelimitedSubjects { get; set; }

        public string CommaSpaceDelimitedSubjects
        {
            get
            {
                var strArray = CommaDelimitedSubjects.Split(',').ToList();
                var str = string.Empty;
                for (int i = 0; i < strArray.Count(); i++)
                {
                    if (i < strArray.Count - 1)
                        str += strArray[i] + ", ";
                    else
                        str += strArray[i];
                }
                return str.Trim();
            }
        }

        public string SpaceDelimitedSubjects
        {
            get
            {
                var strArray = CommaDelimitedSubjects.Split(',').ToList();
                return string.Join(" ", strArray).Trim();
            }
        }
    }
}
