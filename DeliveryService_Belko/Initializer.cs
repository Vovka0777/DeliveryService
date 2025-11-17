using DeliveryService.DAL;
using DeliveryService.DAL.Storage;
using DeliveryService.Domain.Models;
using DeliveryService.Service.Realizations;
using DeliveryService.Service.Interfaces;

namespace DeliveryService_Belko
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
        }
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            services.AddControllersWithViews()
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();
        }
    }
}
