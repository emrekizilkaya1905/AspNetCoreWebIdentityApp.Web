using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Web.Models;
using AspNetCoreWebIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Globalization;

namespace AspNetCoreWebIdentityApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = (await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            var userViewModel = new UserViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
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
        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser is null)
            {
                return Challenge();
            }

            var userEditViewModel = new UserEditViewModel
            {
                UserName = currentUser.UserName ?? string.Empty,
                Email = currentUser.Email ?? string.Empty,
                Phone = currentUser.PhoneNumber ?? string.Empty,
                City = currentUser.City,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender
            };


            ViewBag.gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(userEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser!.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.BirthDate = request.BirthDate;
            currentUser.PhoneNumber = request.Phone;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;

            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";
                var newPicturePath = Path.Combine(wwwrootFolder.FirstOrDefault(x => x.Name == "userpictures")!.PhysicalPath!, randomFileName);
                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await request.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;
            }
            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
                return View();
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "User information updated successfully.";
            var userEditViewModel = new UserEditViewModel
            {
                UserName = currentUser.UserName ?? string.Empty,
                Email = currentUser.Email ?? string.Empty,
                Phone = currentUser.PhoneNumber ?? string.Empty,
                City = currentUser.City,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender
            };
            return View(userEditViewModel);
        }

        // Uyelerin goremedikleri sayfa icin AccessDenied sayfasina yonlendirme yapilmasi icin kullanilir.

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = string.Empty;
            message = "Your account doesn't have the required role to access this page. If you think this is a mistake, please contact your administrator.";
            ViewBag.Message = message;
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
        public async Task<IActionResult> Claims()
        {
            var userClaimList = User.Claims.Select(c => new ClaimViewModel
            {
                Issuer = c.Issuer,
                Type = c.Type,
                Value = c.Value
            }).ToList();
            return View(userClaimList);

        }
    }
}
