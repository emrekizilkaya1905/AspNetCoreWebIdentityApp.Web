using AspNetCoreWebIdentityApp.Service.Services;
using AspNetCoreWebIdentityApp.Core.Models;
using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreWebIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private string userName => User.Identity!.Name!;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<IActionResult> Index()
        {
            var userViewModel = await _memberService.GetUserViewModelAsync(userName);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }
        public async Task Logout()
        {
            await _memberService.LogoutAsync();
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
            var (passwordCheck, result) = await _memberService.ChangePasswordAsync(userName, request.PasswordOld, request.PasswordNew);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors.Select(e => e.Description).ToList());
                return View();
            }
            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Index", "Member");
        }
        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            var model = await _memberService.GetUserEditAsync(userName);
            if (model == null)
            {
                return Challenge();
            }

            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _memberService.UpdateUserAsync(userName, request);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View(request);
            }

            TempData["SuccessMessage"] = "User information updated successfully.";
            return RedirectToAction(nameof(UserEdit));
        }
        // Uyelerin goremedikleri sayfa icin AccessDenied sayfasina yonlendirme yapilmasi icin kullanilir.

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Your account doesn't have the required role to access this page. If you think this is a mistake, please contact your administrator.";
            ViewBag.Message = message;
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult Violence()
        {
            return View();
        }
        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult ExchangePage()
        {
            return View();
        }
        [Authorize(Policy = "StockholmPolicy")]
        public IActionResult StockholmPage()
        {
            return View();
        }
        public IActionResult Claims()
        {
            return View(_memberService.GetClaims(User));
        }
    }
}
