using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Web.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
