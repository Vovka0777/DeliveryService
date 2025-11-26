using DeliveryService.Domain.Filters;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Item;
using System; // Добавлено для Guid
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IItemService
    {
        Task<BaseResponse<List<ItemViewModel>>> GetItems();
        Task<BaseResponse<List<ItemViewModel>>> GetItemsByFilter(ItemFilter filter);

        // НОВЫЙ МЕТОД: Получение одного товара
        Task<BaseResponse<ItemViewModel>> GetItem(Guid id);
    }
}