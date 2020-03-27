using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace Domain.Models.ViewModel
{
    public class UpdateQuestionViewModel : UploadFile
    {
        public Guid QuestionID { get; set; }

        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        public List<Attachment> Attachments { get; set; }

        [AllowHtml]
        [Required]
        public string Description { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
