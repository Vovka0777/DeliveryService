using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Profile;

namespace DeliveryService.Service.Interfaces
{
    public interface IProfileService
    {
        Task<IBaseResponse<ProfileViewModel>> GetProfile(string userName);
        Task<IBaseResponse<ProfileViewModel>> UpdateProfile(ProfileViewModel model);
    }
}