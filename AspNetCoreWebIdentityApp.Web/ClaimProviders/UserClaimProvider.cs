using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Web.ClaimProviders
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Claim identiy: Cookielerin claimden olustugunu soyler.
            var identityUser = (ClaimsIdentity)principal.Identity!;
            var currentUser =await _userManager.FindByNameAsync(identityUser.Name!);

            if(currentUser is null)
            {
                return principal;
            }

            if (String.IsNullOrEmpty(currentUser.City))
            {
                if(!principal.HasClaim(c => c.Type == "city"))
                {
                    identityUser.AddClaim(new Claim("city", currentUser.City));
                }
            }
            return principal;
        }
    }
}
