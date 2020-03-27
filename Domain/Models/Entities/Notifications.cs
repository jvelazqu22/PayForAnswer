using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Entities.Identity;

namespace Domain.Models.Entities
{
    public class Notifications
    {
        public Notifications()
        {
            NewAnswerToMyQuestion = true;
            NewQuestionRelatedToMySubjects = true;
            NewComment = true;
            QuestionStatusChangeWithMyAnswers = false;
        }
        [Key, ForeignKey("User")]
        public int UserID { get; set; }

        public bool NewAnswerToMyQuestion { get; set; }
        public bool NewQuestionRelatedToMySubjects { get; set; }
        public bool NewComment { get; set; }
        public bool QuestionStatusChangeWithMyAnswers { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
