using DeliveryService.DAL;
using DeliveryService.DAL.Storage;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb; // Добавь для UserDb
using DeliveryService.Service.Realizations;
using DeliveryService.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService_Belko
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
            services.AddScoped<IBaseStorage<Item>, ItemStorage>();
            // Если создавал BasketStorage, добавь его сюда, но пока мы работали через DbContext напрямую в сервисах
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IItemService, ItemService>();

            // --- ДОБАВЛЯЕМ НОВЫЕ СЕРВИСЫ ---
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProfileService, ProfileService>();
            // -------------------------------

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}