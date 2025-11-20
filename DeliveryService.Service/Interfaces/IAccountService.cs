using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using System.Security.Claims;

namespace DeliveryService.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> Register(User model);
        Task<BaseResponse<ClaimsIdentity>> Login(User model);
        Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode);
    }
}