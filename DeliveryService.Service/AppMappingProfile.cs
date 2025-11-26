using AutoMapper;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
using DeliveryService.Domain.ViewModels.Catalog; // Если нужно
using DeliveryService.Domain.ViewModels.Item;    // Важно!
using DeliveryService.Domain.ViewModels.LoginAndRegistration;

namespace DeliveryService.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // 1. Маппинг БД <-> Доменная модель
            CreateMap<ItemDb, Item>().ReverseMap();
            CreateMap<UserDb, User>().ReverseMap();

            // 2. ИСПРАВЛЕНО: Маппинг Item -> ItemViewModel (а не CatalogViewModel)
            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => GetCategoryName(src.Category)));

            // 3. Остальные маппинги
            CreateMap<LoginViewModel, User>();
            CreateMap<RegisterViewModel, User>();
        }

        private string GetCategoryName(int categoryId)
        {
            return categoryId switch
            {
                0 => "Еда",
                1 => "Канцелярия",
                2 => "Стройматериалы",
                3 => "Одежда",
                _ => "Разное"
            };
        }
    }
}