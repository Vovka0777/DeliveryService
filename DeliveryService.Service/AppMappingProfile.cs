using AutoMapper;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
using DeliveryService.Domain.ViewModels.Catalog;
using DeliveryService.Domain.ViewModels.Item;
using DeliveryService.Domain.ViewModels.LoginAndRegistration;
using DeliveryService.Domain.ViewModels.Profile; 

namespace DeliveryService.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ItemDb, Item>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (ItemType)src.Category))
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => (int)src.Category));

            CreateMap<UserDb, User>().ReverseMap();

            CreateMap<Item, ItemViewModel>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<ItemViewModel, Item>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));

            CreateMap<LoginViewModel, User>();
            CreateMap<RegisterViewModel, User>();

            CreateMap<UserDb, User>()
               .ForMember(dest => dest.Basket, opt => opt.MapFrom(src => src.Basket))
               .MaxDepth(1);
        }
    }
}