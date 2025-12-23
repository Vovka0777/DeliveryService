using DeliveryService.Domain.Filters;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Item;
using System; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IItemService
    {
        Task<BaseResponse<List<ItemViewModel>>> GetItems();
        Task<BaseResponse<List<ItemViewModel>>> GetItemsByFilter(ItemFilter filter);
        Task<BaseResponse<ItemViewModel>> GetItem(Guid id);
        Task<BaseResponse<ItemViewModel>> Create(ItemViewModel model);
        Task<BaseResponse<ItemViewModel>> Edit(Guid id, ItemViewModel model);
        Task<BaseResponse<bool>> Delete(Guid id);
    }
}