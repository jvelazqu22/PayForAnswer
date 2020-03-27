using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Account
{
    public class ForgotUserNameOrPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Email { get; set; }
    }
}
