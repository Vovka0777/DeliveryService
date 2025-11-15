using AutoMapper;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ViewModels.LoginAndRegistration;
namespace DeliveryService.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User, UserDb>().ReverseMap();
            CreateMap<User, LoginViewModel>().ReverseMap();
            CreateMap<User, RegisterViewModel>().ReverseMap();
        }
    }
}
