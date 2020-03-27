using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Account
{
    public class ProfileViewModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "New Email Address")]
        public string NewEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm New Email Address")]
        [Compare("NewEmail", ErrorMessage = "The Email Address and confirmation Email Address do not match.")]
        public string ConfirmNewEmail { get; set; }

    }
}
