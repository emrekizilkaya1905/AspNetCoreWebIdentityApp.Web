using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebIdentityApp.Web.Areas.Admin.Models
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [Display(Name = "Role Name")]
        public string Name { get; set; }=null!;
    }
}
