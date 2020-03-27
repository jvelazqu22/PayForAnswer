using Domain.App_GlobalResources;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Account
{
    public class BooleanRequiredAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value;
        }
    }

    public class RegisterModel
    {
        public RegisterModel() { IsFemale = false; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        //TODO: make password stronger
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email Address")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Compare("EmailAddress", ErrorMessage = "The Email Address and confirmation Email Address do not match.")]
        public string ConfirmEmailAddress { get; set; }

        public string RegistrationToken { get; set; }

        public string FullName { get; set; }

        [Required]
        public bool IsFemale { get; set; }

        [Required]
        public int Day { get; set; }
        [Required]
        public int Month { get; set; }
        [Required]
        public int Year { get; set; }


        //[Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions.")]
        [BooleanRequiredAttribute]
        [Display(Name = "Terms and conditions & Privacy policy")]
        public bool TermsAndConditionsAccepted { get; set; }
    }
}
