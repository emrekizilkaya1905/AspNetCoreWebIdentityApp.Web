using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Web.Models
{
    public class AppUser:IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDay { get; set; }
        public byte? Gender { get; set; }
    }
}
