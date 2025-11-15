using DeliveryService.Domain.Helpers;
namespace DeliveryService.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ServiceModels.User, Models.User>().ReverseMap();
        }
    }
}