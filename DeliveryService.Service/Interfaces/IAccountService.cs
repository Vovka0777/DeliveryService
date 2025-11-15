using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;

namespace DeliveryService.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<User>> Register(User userModel);
        Task<BaseResponse<User>> Login(User userModel);
    }
}