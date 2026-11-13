using AspNetCoreWebIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebIdentityApp.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        [Display(Name = "User Name :")]
        public string UserName { get; set; } = null!;
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Phone :")]
        public string Phone { get; set; } = null!;

        [Display(Name = "Birth Date :")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "City  :")]
        public string? City { get; set; }

        [Display(Name = "Profile Picture :")]
        public IFormFile? Picture { get; set; }
        [Display(Name = "Gender :")]
        public Gender? Gender { get; set; }
    }
}
