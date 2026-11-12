using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebIdentityApp.Web.ViewModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel()
        {

        }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name :")]
        public string UserName { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Phone :")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password :")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = null!;
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password :")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
