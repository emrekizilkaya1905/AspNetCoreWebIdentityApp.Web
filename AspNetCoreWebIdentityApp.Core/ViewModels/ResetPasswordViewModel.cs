using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebIdentityApp.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "New Password :")]
        public string Password { get; set; } = null!;
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "New Confirm Password :")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
