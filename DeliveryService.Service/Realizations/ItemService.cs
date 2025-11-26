using AutoMapper;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.ViewModels.Item;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeliveryService.DAL;

namespace DeliveryService.Service.Realizations
{
    public class ItemService : IItemService
    {
        private readonly IBaseStorage<Item> _itemStorage;
        private readonly IMapper _mapper;

        public ItemService(IBaseStorage<Item> itemStorage, IMapper mapper)
        {
            _itemStorage = itemStorage;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<ItemViewModel>>> GetItems()
        {
            try
            {
                // Получаем все записи из таблицы Items
                var items = await _itemStorage.GetAll().ToListAsync();

                // Преобразуем (маппим) модели БД в модели представления
                var result = _mapper.Map<List<ItemViewModel>>(items);

                return new BaseResponse<List<ItemViewModel>>()  
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ItemViewModel>>()
                {
                    Description = $"[GetItems] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}