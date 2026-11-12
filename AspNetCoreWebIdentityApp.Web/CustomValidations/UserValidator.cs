using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Web.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();
            var isDigit = user.UserName is { Length: > 0 } userName && int.TryParse(userName[0].ToString(), out _);
            if (isDigit)
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameStartsWithNumber",
                    Description = "UserName cannot start with number."
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
