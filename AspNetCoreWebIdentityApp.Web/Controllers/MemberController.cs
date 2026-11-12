using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Web.Models;
using AspNetCoreWebIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel()
            {
                UserName = currentUser!.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber
            };
            return View(userViewModel);
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var passwordCheck = await _userManager.CheckPasswordAsync(currentUser!, request.PasswordOld);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                return View();
            }   
            var result = await _userManager.ChangePasswordAsync(currentUser!, request.PasswordOld, request.PasswordNew);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(e => e.Description).ToList());    
                return View();
            }
            //Security stampida degistirmemiz lazim.
            await _userManager.UpdateSecurityStampAsync(currentUser!);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser!, request.PasswordNew, true, false);
            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Index", "Member");
        }
    }
}
