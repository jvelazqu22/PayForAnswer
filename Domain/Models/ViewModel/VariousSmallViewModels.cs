using Domain.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.ViewModel
{
    public class QMarketingHistoryViewModel
    {
        public Guid? QuestionID { get; set; }
    }

    public class SiteFeedbackViewModel
    {
        [Required, StringLength(Size.FeedbackTitleMaxCharacters)]
        [MaxLength(Size.FeedbackTitleMaxCharacters)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
