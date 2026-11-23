using AspNetCoreWebIdentityApp.Core.PermissionRoot;
using AspNetCoreWebIdentityApp.Repository.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreWebIdentityApp.Repository.Seeds
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
                    Permissions.Stock.Read,
                    Permissions.Order.Read,
                    Permissions.Catalog.Read);
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
                    Permissions.Stock.Read,
                    Permissions.Stock.Create,
                    Permissions.Stock.Update,
                    Permissions.Order.Read,
                    Permissions.Order.Create,
                    Permissions.Order.Update,
                    Permissions.Catalog.Read,
                    Permissions.Catalog.Create,
                    Permissions.Catalog.Update);
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
                    Permissions.Stock.Read,
                    Permissions.Stock.Create,
                    Permissions.Stock.Update,
                    Permissions.Stock.Delete,
                    Permissions.Order.Read,
                    Permissions.Order.Create,
                    Permissions.Order.Update,
                    Permissions.Order.Delete,
                    Permissions.Catalog.Read,
                    Permissions.Catalog.Create,
                    Permissions.Catalog.Update,
                    Permissions.Catalog.Delete);
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