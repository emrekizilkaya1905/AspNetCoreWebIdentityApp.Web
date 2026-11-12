using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Web.CustomValidations
{
    public class PasswordValidator:IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (password is null)
            {
                return Task.FromResult(IdentityResult.Failed(manager.ErrorDescriber.PasswordTooShort(manager.Options.Password.RequiredLength)));
            }
            if (!string.IsNullOrEmpty(user.UserName) && password.Contains(user.UserName, StringComparison.OrdinalIgnoreCase))
            {
               errors.Add(new IdentityError()
               {
                   Code = "PasswordContainsUserName",
                   Description = "Password cannot contain username."
               });
            }
            if (password.StartsWith("1234", StringComparison.Ordinal))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordStartsWith1234",
                    Description = "Password cannot start with 1234."
                });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    
    }
}
