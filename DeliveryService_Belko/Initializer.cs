using DeliveryService.DAL;
using DeliveryService.DAL.Storage;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
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
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IItemService, ItemService>();

            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProfileService, ProfileService>();

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();
        }
    }
}