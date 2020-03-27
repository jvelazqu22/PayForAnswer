using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<int>
    {
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New Email Address")]

        public string NewEmail { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        public DateTime? DoB { get; set; }

        public bool? IsFemale { get; set; }

        public bool AcceptedTermsConditionsAndPrivacyPolicy { get; set; }

        [NotMapped]
        public string ChangePasswordToken { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual Notifications Notifications { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
