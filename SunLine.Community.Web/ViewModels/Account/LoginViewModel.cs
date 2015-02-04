using System.ComponentModel.DataAnnotations;

namespace SunLine.Community.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}