using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Repository.Models;
using AspNetCoreWebIdentityApp.Web.Services;
using AspNetCoreWebIdentityApp.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, IEmailService emailService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityResult = await _userManager.CreateAsync(new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone
            }, request.PasswordConfirm);

            if (!identityResult.Succeeded)
            {
                //Ekledigimiz extension method sayesinde, ModelState.AddModelError() methodunu tek tek cagirmak yerine, bir listeyi tek seferde ekleyebiliriz.
                ModelState.AddModelErrorList(identityResult.Errors.Select(e => e.Description).ToList());
                return View();
            }

            // Kullanici olusturulduktan sonra, kullaniciya 30 gunluk bir ExchangeExpire claim ekliyoruz.
            var exchangeExpireClaim = new Claim("ExchangeExpire", DateTime.Now.AddDays(30).ToString());
            var user = await _userManager.FindByEmailAsync(request.Email);
            var claimResult = await _userManager.AddClaimAsync(user!, exchangeExpireClaim);

            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(claimResult.Errors.Select(e => e.Description).ToList());
                return View();
            }
            TempData["SuccessMessage"] = "User Created Successfully";
            return RedirectToAction(nameof(HomeController.SignUp));
        }
        public IActionResult SignIn()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // urli kullandik cunku kullanici login olduktan sonra, login olmadan onceki sayfaya donmesini istiyoruz.
            returnUrl = returnUrl ?? Url.Action("Index", "Home");
            var hasUser = await _userManager.FindByEmailAsync(model.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong");
                return View();
            }
            //Ispersistent: kullancinin bilgilerini cookie de tutulup tutmayacagini belirler. RememberMe: kullanici bilgilerini hatirlamasini(bizim belirledigimiz zure) istiyorsa true, istemiyorsa false olarak ayarlanir.
            //4.parametre : lockoutOnFailure: kullanici bilgilerini yanlis girerse, kullaniciya lockout uygulanip uygulanmayacagini belirler. Biz false olarak ayarladik.
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);
            // Kullanici lockout olursa
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string> { "Your account is locked out for 3 minutes" });
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string> { $"Email or Password is wrong ", $"AccessFailedCount: {await _userManager.GetAccessFailedCountAsync(hasUser)}" });
                return View();
            }
            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });
               
            }
            return Redirect(returnUrl!);
        }

        public IActionResult ForgetPassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {

            var hasUser = await _userManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "We could not find an account with that email.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            // link
            //https://localhost:7228/Home/ResetPassword?userId=1&token=123456
            var passwordResetLink = Url.Action("ResetPassword", "Home", new
            {
                userId = hasUser.Id,
                token = passwordResetToken
            }, HttpContext.Request.Scheme);

            // MailService ile mail atilabilir.

            await _emailService.SendResetPasswordEmail(passwordResetLink!, hasUser.Email!);

            TempData["SuccessMessage"] = "Password reset link has been sent to your email. Please check your email.";
            return RedirectToAction(nameof(ForgetPassword));
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["UserId"] = userId;
            TempData["Token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["UserId"];
            var token = TempData["Token"];
            if (userId == null || token == null)
            {
                throw new Exception("Something went wrong with the password reset request.");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hasUser = await _userManager.FindByIdAsync(userId.ToString()!);
            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "We could not find an account with that email.");
                return View();
            }
            var resetPasswordResult = await _userManager.ResetPasswordAsync(hasUser, token.ToString()!, model.Password);
            if (resetPasswordResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfully. You can now log in with your new password.";
                return RedirectToAction(nameof(SignIn));
            }
            ModelState.AddModelErrorList(resetPasswordResult.Errors.Select(e => e.Description).ToList());
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
