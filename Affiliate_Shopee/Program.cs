using Affiliate_Shopee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Affiliate_Shopee.Data;
using Affiliate_Shopee.Areas.Identity.Data;
using Microsoft.AspNetCore.Session;
using Affiliate_Shopee.Services;
using Affiliate_Shopee.Validators;
using Affiliate_Shopee.Areas.Identity.Pages.Account.Manage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<Affiliate_ShopeeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ShopeeAffContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<Affiliate_ShopeeUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.AllowedUserNameCharacters = null;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<Affiliate_ShopeeContext>()
.AddUserValidator<CustomUserValidator<Affiliate_ShopeeUser>>();

builder.Services.AddScoped<IUserValidator<Affiliate_ShopeeUser>, CustomUserValidator<Affiliate_ShopeeUser>>();
builder.Services.AddScoped<CustomerUserManager>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IEmailSender, EmailSender>(serviceProvider =>
    new EmailSender(new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            { "Smtp:Host", builder.Configuration["Smtp:Host"] },
            { "Smtp:Port", builder.Configuration["Smtp:Port"] },
            { "Smtp:Username", builder.Configuration["Smtp:Username"] },
            { "Smtp:Password", builder.Configuration["Smtp:Password"] }
        }).Build())
);

builder.Services.AddSingleton<OtpService>();

// Configure session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = ".AspNetCore.Session";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var basePath = AppContext.BaseDirectory;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapAreaControllerRoute(
    name: "Identity",
    areaName: "Identity",
    pattern: "Identity/{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller}/{action}/{id?}");
app.MapAreaControllerRoute(
    name: "Seller",
    areaName: "Seller",
    pattern: "Seller/{controller}/{action}/{id?}/{id1?}/{id2?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{id1?}");
app.MapRazorPages();

app.Run();
