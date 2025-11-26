using DeliveryService.Domain.Filters; // Подключаем фильтры
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Item;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryService.Service.Interfaces
{
    public interface IItemService
    {
        // Метод для получения всех товаров (уже есть)
        Task<BaseResponse<List<ItemViewModel>>> GetItems();

        // НОВЫЙ МЕТОД: Получение товаров с фильтрацией
        Task<BaseResponse<List<ItemViewModel>>> GetItemsByFilter(ItemFilter filter);
    }
}