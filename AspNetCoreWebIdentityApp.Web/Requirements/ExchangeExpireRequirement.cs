using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Web.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
    }
    // Bu generici gecmemizin nedeni program.csden parametre atayabilmek.
    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            //Kullanicin once login olmasi gerekiyor. Bu yuzden claim kontrolu yapabiliriz.

            // null ise
            if (!context.User.HasClaim(x => x.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            // varsa
            Claim exchangeExpireDate = context.User.FindFirst(x => x.Type == "ExchangeExpireDate")!;

            if (DateTime.Now > Convert.ToDateTime(exchangeExpireDate.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
