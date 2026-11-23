using System.Globalization;
using System.Security.Claims;
using AspNetCoreWebIdentityApp.Core.ViewModels;
using AspNetCoreWebIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Service.Services
{
    public class MemberService : IMemberService
    {
        private readonly IFileService _fileService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileService = fileService;
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(currentUser, password);
        }
        public async Task<(bool IsPasswordCorrect, IdentityResult Result)> ChangePasswordAsync(string userName, string passwordOld, string passwordNew)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
            {
                return (false, IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found."
                }));
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(currentUser, passwordOld);
            if (!passwordCheck)
            {
                return (false, IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordMismatch",
                    Description = "The current password is incorrect."
                }));
            }

            var result = await _userManager.ChangePasswordAsync(currentUser, passwordOld, passwordNew);
            if (!result.Succeeded)
            {
                return (true, result);
            }

            var stampResult = await _userManager.UpdateSecurityStampAsync(currentUser);
            if (!stampResult.Succeeded)
            {
                return (true, stampResult);
            }

            await _signInManager.SignOutAsync();
            var signInResult = await _signInManager.PasswordSignInAsync(currentUser, passwordNew, true, false);
            if (!signInResult.Succeeded)
            {
                return (true, IdentityResult.Failed(new IdentityError
                {
                    Code = "SignInFailed",
                    Description = "Your password was changed, but automatic sign-in failed. Please sign in again."
                }));
            }

            return (true, result);
        }
        public async Task<UserEditViewModel?> GetUserEditAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            return new UserEditViewModel
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                City = user.City,
                BirthDate = user.BirthDate,
                Gender = user.Gender
            };
        }

        public async Task<IdentityResult> UpdateUserAsync(string userName, UserEditViewModel request)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "User not found." });
            }

            user.UserName = request.UserName;
            user.Email = request.Email;
            user.PhoneNumber = request.Phone;
            user.City = request.City;
            user.BirthDate = request.BirthDate;
            user.Gender = request.Gender;

            if (request.Picture != null && request.Picture.Length > 0)
            {
                user.Picture = await _fileService.SaveUserPictureAsync(request.Picture);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }

            var stampResult = await _userManager.UpdateSecurityStampAsync(user);
            if (!stampResult.Succeeded)
            {
                return stampResult;
            }

            await _signInManager.SignOutAsync();
            if (user.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(user, true, new[]
                {
                    new Claim("birthdate", user.BirthDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                });
            }
            else
            {
                await _signInManager.SignInAsync(user, true);
            }

            return result;
        }

        public List<ClaimViewModel> GetClaims(ClaimsPrincipal user)
        {
            return user.Claims.Select(claim => new ClaimViewModel
            {
                Issuer = claim.Issuer,
                Type = claim.Type,
                Value = claim.Value
            }).ToList();
        }
        public Task LogoutAsync()
        {
            return _signInManager.SignOutAsync();
        }
        public async Task<UserViewModel?> GetUserViewModelAsync(string userName)
        {
            var currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
            {
                return null;
            }

            return new UserViewModel
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
        }
    }
}