using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AspNetCoreWebIdentityApp.Core.ViewModels;

namespace AspNetCoreWebIdentityApp.Service.Services
{
    public interface IMemberService
    {
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<(bool IsPasswordCorrect, IdentityResult Result)> ChangePasswordAsync(string userName, string passwordOld, string passwordNew);
        Task<UserEditViewModel?> GetUserEditAsync(string userName);
        Task<IdentityResult> UpdateUserAsync(string userName, UserEditViewModel request);
        List<ClaimViewModel> GetClaims(ClaimsPrincipal user);
        Task LogoutAsync();
        Task<UserViewModel?> GetUserViewModelAsync(string userName);
    }
}