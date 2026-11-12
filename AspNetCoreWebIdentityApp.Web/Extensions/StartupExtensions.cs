using AspNetCoreWebIdentityApp.Web.CustomValidations;
using AspNetCoreWebIdentityApp.Web.Localizations;
using AspNetCoreWebIdentityApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreWebIdentityApp.Web.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentiyWithExt(this IServiceCollection services)
        {
            //Token time Life time
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                //Lockout ayarlari
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddPasswordValidator<PasswordValidator>().
            AddUserValidator<UserValidator>().
            AddErrorDescriber<LocalizationIdentityErrorDescriber>().
            AddDefaultTokenProviders().
            AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
