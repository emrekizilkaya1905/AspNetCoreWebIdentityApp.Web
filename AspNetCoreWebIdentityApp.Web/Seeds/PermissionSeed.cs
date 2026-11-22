using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Web.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");
            if (!hasBasicRole)
            {
                await roleManager.CreateAsync(new AppRole { Name = "BasicRole" });

                var basicRole = await roleManager.FindByNameAsync("BasicRole");

                await AddPermissionsAsync(roleManager, basicRole!,
                    PermissionRoot.Permissions.Stock.Read,
                    PermissionRoot.Permissions.Order.Read,
                    PermissionRoot.Permissions.Catalog.Read);
            }
            var hasAdvancedRole = await roleManager.RoleExistsAsync("AdvancedRole");
            if (!hasAdvancedRole)
            {
                var advancedRole = new AppRole { Name = "AdvancedRole" };
                var result = await roleManager.CreateAsync(advancedRole);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
                }

                await AddPermissionsAsync(roleManager, advancedRole,
                    PermissionRoot.Permissions.Stock.Read,
                    PermissionRoot.Permissions.Stock.Create,
                    PermissionRoot.Permissions.Stock.Update,
                    PermissionRoot.Permissions.Order.Read,
                    PermissionRoot.Permissions.Order.Create,
                    PermissionRoot.Permissions.Order.Update,
                    PermissionRoot.Permissions.Catalog.Read,
                    PermissionRoot.Permissions.Catalog.Create,
                    PermissionRoot.Permissions.Catalog.Update);
            }
            var hasAdminRole = await roleManager.RoleExistsAsync("AdminRole");
            if (!hasAdminRole)
            {
                var adminRole = new AppRole { Name = "AdminRole" };
                var result = await roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
                }

                await AddPermissionsAsync(roleManager, adminRole,
                    PermissionRoot.Permissions.Stock.Read,
                    PermissionRoot.Permissions.Stock.Create,
                    PermissionRoot.Permissions.Stock.Update,
                    PermissionRoot.Permissions.Stock.Delete,
                    PermissionRoot.Permissions.Order.Read,
                    PermissionRoot.Permissions.Order.Create,
                    PermissionRoot.Permissions.Order.Update,
                    PermissionRoot.Permissions.Order.Delete,
                    PermissionRoot.Permissions.Catalog.Read,
                    PermissionRoot.Permissions.Catalog.Create,
                    PermissionRoot.Permissions.Catalog.Update,
                    PermissionRoot.Permissions.Catalog.Delete);
            }
        }

        private static async Task AddPermissionsAsync(RoleManager<AppRole> roleManager, AppRole role, params string[] permissions)
        {
            foreach (var permission in permissions)
            {
                var result = await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
                }
            }
        }
    }
}