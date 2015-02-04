using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Language")]
        public IList<Language> Languages { get; set; }

        public string RecaptchaPublicKey { get; set; }
    }
}