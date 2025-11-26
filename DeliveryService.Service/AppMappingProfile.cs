using AutoMapper;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
using DeliveryService.Domain.ViewModels.Item;

namespace DeliveryService.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Маппинг из БД в Доменную модель и обратно
            CreateMap<ItemDb, Item>().ReverseMap();

            // Маппинг из Доменной модели во View (для сайта)
            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => GetCategoryName(src.Category))); // Превращаем число в текст
        }

        // Вспомогательный метод для получения названия категории
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