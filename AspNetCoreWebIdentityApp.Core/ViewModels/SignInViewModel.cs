using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace AspNetCoreWebIdentityApp.Core.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {

        }
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
