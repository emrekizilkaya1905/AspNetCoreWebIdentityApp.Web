using AspNetCoreWebIdentityApp.Web.ClaimProviders;
using AspNetCoreWebIdentityApp.Web.Extensions;
using AspNetCoreWebIdentityApp.Web.Models;
using AspNetCoreWebIdentityApp.Web.OptionModels;
using AspNetCoreWebIdentityApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // SecurityStampValidatorOptions ayarları, kullanıcının kimlik doğrulama durumunu kontrol etmek için kullanılır.
    // ValidateInterval özelliği, kullanıcının kimlik doğrulama durumunun ne sıklıkla kontrol edileceğini belirler.
    // Bu sayede, kullanıcı oturumunun geçerliliği düzenli aralıklarla kontrol edilir ve gerekirse oturum sonlandırılır.
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddIdentiyWithExt();
// Define policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StockholmPolicy", policy =>
    {
        policy.RequireClaim("city", "Stockholm");
    });
});


// Write cookie options
builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "AspNetCoreWebIdentityAppCookie";
    options.Cookie = cookieBuilder;
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(60);
    // SlidingExpiration özelliği, kullanıcının oturumunun süresini uzatmak için kullanılır. Eğer kullanıcı aktifse, oturum süresi uzatılır.
    // Bu sayede, kullanıcı oturumunu kaybetmeden uygulamayı kullanmaya devam edebilir.
    options.SlidingExpiration = true;
    options.LoginPath = new PathString("/Home/SignIn");
    options.LogoutPath = new PathString("/Member/Logout");
    options.AccessDeniedPath = new PathString("/Member/AccessDenied");
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
