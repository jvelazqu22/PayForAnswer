using Domain.Constants;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Models.Table.Entities.Question
{
    public class UserQuestion : TableEntity
    {

        public UserQuestion()
        {
            this.PartitionKey = UserName + "_" + StatusID;
            this.RowKey = QuestionID.ToString();
        }

        public string UserName { get; set; }

        public string StatusID { get; set; }

        public Guid QuestionID { get; set; }

        public string CommaDelimetedSubjects { get; set; }

        [Required, StringLength(Size.GoogleDescrLineMaxCharacters)]
        public string Title { get; set; }

        [AllowHtml]
        [UIHint("tinymce_classic")]
        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Amount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

