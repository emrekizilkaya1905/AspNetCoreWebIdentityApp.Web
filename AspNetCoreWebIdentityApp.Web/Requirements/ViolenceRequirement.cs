using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Web.Requirements
{
    public class ViolenceRequirement: IAuthorizationRequirement
    {
        public int Age { get; set; }
    }
    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == "Birt"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim ageClaim = context.User.FindFirst(x => x.Type == "Age")!;
            if (Convert.ToInt32(ageClaim.Value) >= requirement.Age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
