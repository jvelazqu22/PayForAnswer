using Domain.Models.Helper;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Models.ViewModel
{
    public class NewAnswerViewModel : UploadFile
    {
        public Guid QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public string EmailAddressOfUserWhoPostedQuestion { get; set; }

        [UIHint("tinymce_classic")]
        [AllowHtml]
        public string NewPostedAnswer { get; set; }

        public bool NewAnswerToMyQuestion { get; set; }

        public int QuestionUserID { get; set; }
    }
}
