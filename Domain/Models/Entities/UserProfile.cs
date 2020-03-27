using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(200)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(200)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New Email Address")]
        public string NewEmail { get; set; }
        public bool ReplyToMyQuestionNotification { get; set; }
        public bool NewQuestionRelatedToMySubjectsNotification { get; set; }
        public bool QuestionStatusChangeWithMyAnswers { get; set; }

        [NotMapped]
        public string ChangePasswordToken { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
