using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Order;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IBaseResponse<CartViewModel>> GetCart(string userName);
        Task<IBaseResponse<bool>> AddToCart(Guid itemId, string userName);
        Task<IBaseResponse<bool>> RemoveFromCart(Guid itemId, string userName);
        Task<IBaseResponse<bool>> ConfirmOrder(string userName); // Для оформления
    }
}