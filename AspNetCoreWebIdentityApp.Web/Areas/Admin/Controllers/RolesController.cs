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
            TempData["SuccessMessage"] = "Role created successfully.";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _roleManager.FindByIdAsync(id);
            if (roleToDelete == null)
            {
                throw new Exception("Role not found");
            }
            var result = await _roleManager.DeleteAsync(roleToDelete);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.Select(x => x.Description).First());
            }
            TempData["SuccessMessage"] = "Role deleted successfully.";
            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            if (currentUser == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            var roleViewModelList = new List<AssignRoleToUserViewModel>();

            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel
                {
                    Id = role.Id,
                    Name = role.Name!,
                };
                if (userRoles.Contains(role.Name!))
                {
                    assignRoleToUserViewModel.Exist = true;
                }
                roleViewModelList.Add(assignRoleToUserViewModel);
            }

            ViewBag.UserId = currentUser.Id;
            ViewBag.UserName = currentUser.UserName;
            return View(roleViewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(List<AssignRoleToUserViewModel> requestList, string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            foreach (var role in requestList)
            {
                if (role.Exist && !userRoles.Contains(role.Name))
                {
                    await _userManager.AddToRoleAsync(currentUser, role.Name);
                }
                else if (!role.Exist && userRoles.Contains(role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(currentUser, role.Name);
                }
            }
            TempData["SuccessMessage"] = "Roles updated successfully.";
            return RedirectToAction(nameof(HomeController.UserList), "Home", new { area = "Admin" });
        }
    }
}
