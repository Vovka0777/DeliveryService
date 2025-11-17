using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using System.Security.Claims;

namespace DeliveryService.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<ClaimsIdentity>> Register(User model);
        Task<BaseResponse<ClaimsIdentity>> Login(User model);
    }
}