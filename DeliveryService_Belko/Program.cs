using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DeliveryService.DAL;

var builder = WebApplication.CreateBuilder(args);


string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(connection));

builder.Services.AddControllersWithViews(); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Маппинг контроллеров
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=SiteInformation}/{id?}");

app.Run();