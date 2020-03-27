using Domain.Constants;
using Domain.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Feedback
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(Size.FeedbackTitleMaxCharacters)]
        public string Title { get; set; }

        public string FeedbackUrl { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
