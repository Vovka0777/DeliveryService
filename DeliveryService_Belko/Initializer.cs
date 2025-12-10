using DeliveryService.DAL;
using DeliveryService.DAL.Storage;
using DeliveryService.Domain.Models;
using DeliveryService.Service.Realizations;
using DeliveryService.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection; // Не забудьте этот using, если он нужен для IServiceCollection

namespace DeliveryService_Belko
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();

            // Добавьте эту строку:
            services.AddScoped<IBaseStorage<Item>, ItemStorage>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IItemService, ItemService>();

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}