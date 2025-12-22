using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;

namespace DeliveryService.Service.Interfaces
{
    public interface ICartService
    {
        Task<IBaseResponse<IEnumerable<BasketItem>>> GetItems(string userName);
        Task<IBaseResponse<Basket>> AddItem(string userName, Guid itemId);
        Task<IBaseResponse<bool>> RemoveItem(string userName, Guid itemId);
        Task<IBaseResponse<bool>> ClearBasket(string userName);
    }
}