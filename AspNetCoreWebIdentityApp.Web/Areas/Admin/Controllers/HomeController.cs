using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebIdentityApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userViewModelList= userList.Select(u => new Models.UserViewModel
            {
                Id = u.Id,
                Name = u.UserName,
                Email = u.Email
            }).ToList();
            return View(userViewModelList);
        }
    }
}
