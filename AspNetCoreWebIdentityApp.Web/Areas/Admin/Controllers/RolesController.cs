using AspNetCoreWebIdentityApp.Web.Areas.Admin.Models;
using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebIdentityApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;
        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name!
            }).ToListAsync();

            return View(roles);
        }

        public IActionResult RoleCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            if (ModelState.IsValid)
            {
                var role = new AppRole { Name = request.Name };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelErrorList(result.Errors);
                    return View();
                }
            }
            return RedirectToAction(nameof(RolesController.Index));
        }
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await _roleManager.FindByIdAsync(id);

            if (roleToUpdate == null)
            {
                throw new Exception("Updated role not found");

            }
            return View(new RoleUpdateViewModel
            {
                Id = roleToUpdate.Id,
                Name = roleToUpdate.Name!
            });
        }
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var roleToUpdate = await _roleManager.FindByIdAsync(request.Id);
            if (roleToUpdate == null)
            {
                throw new Exception("Updated role not found");
            }

            roleToUpdate.Name = request.Name;
            var result = await _roleManager.UpdateAsync(roleToUpdate);
            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View(request);
            }

            TempData["SuccessMessage"] = "Role updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
