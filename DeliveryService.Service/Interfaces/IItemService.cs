using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Item;

namespace DeliveryService.Service.Interfaces
{
    public interface IItemService
    {
        Task<BaseResponse<List<ItemViewModel>>> GetItems();
    }
}