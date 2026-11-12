using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebIdentityApp.Web.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password :")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordOld { get; set; }=null!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New Password is required")]
        [Display(Name = "New Password :")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordNew { get; set; }=null!;
        [Compare(nameof(PasswordNew), ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "New Confirm Password :")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordNewConfirm { get; set; }= null!;

    }
}
