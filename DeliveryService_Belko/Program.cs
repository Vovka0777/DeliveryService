using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DeliveryService.DAL;
using DeliveryService_Belko;
using AutoMapper;
using DeliveryService.Service.Interfaces;
using DeliveryService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using DeliveryService.Domain.Validator;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseNpgsql(connection));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
      options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Logout");
  })

.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
    options.Scope.Add("profile");

    options.ClaimActions.MapJsonKey("picture", "picture");
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AppMappingProfile>();
});


builder.Services.AddControllersWithViews()
.AddDataAnnotationsLocalization()
.AddViewLocalization();

builder.Services.AddScoped<UserValidator>();

builder.Services.InitializeRepositories();  
builder.Services.InitializeServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=SiteInformation}/{id?}");

app.Run();