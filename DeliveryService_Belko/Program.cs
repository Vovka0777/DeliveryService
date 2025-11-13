using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DeliveryService.DAL; // Укажите свой Namespace

var builder = WebApplication.CreateBuilder(args);

// --- Сервисы ---

// Добавляем ApplicationDbContext как сервис, используя строку подключения
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Строка 12, которая вызывала ошибку, теперь будет работать:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseNpgsql(connection));

// Добавление MVC/Controller сервисов
builder.Services.AddControllersWithViews(); // Если это MVC
// Если вы используете API, то builder.Services.AddControllers();

var app = builder.Build();

// --- Конфигурация Pipeline ---

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
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