using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Filters; // Важно!
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Domain.ViewModels.Item;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // ... Ваш метод GetItems оставьте без изменений ...
        public async Task<BaseResponse<List<ItemViewModel>>> GetItems()
        {
            // (Ваш старый код)
            try
            {
                var items = await _itemStorage.GetAll().ToListAsync();
                var result = _mapper.Map<List<ItemViewModel>>(items);
                return new BaseResponse<List<ItemViewModel>>() { Data = result, StatusCode = StatusCode.OK };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ItemViewModel>>() { Description = ex.Message, StatusCode = StatusCode.InternalServerError };
            }
        }

        // === НОВАЯ РЕАЛИЗАЦИЯ ===
        public async Task<BaseResponse<List<ItemViewModel>>> GetItemsByFilter(ItemFilter filter)
        {
            try
            {
                // 1. Получаем все товары из базы данных
                var items = await _itemStorage.GetAll().ToListAsync();

                // 2. Фильтрация по Цене
                // Если MaxPrice > 0, берем только те, что дешевле или равны
                if (filter.MaxPrice > 0)
                {
                    items = items.Where(x => x.Price <= filter.MaxPrice).ToList();
                }

                // 3. Фильтрация по Категориям
                // Если список категорий не пуст, берем только те, что в списке
                if (filter.Categories != null && filter.Categories.Any())
                {
                    items = items.Where(x => filter.Categories.Contains((int)x.Category)).ToList();
                }

                // 4. Сортировка
                if (!string.IsNullOrEmpty(filter.Ordering))
                {
                    switch (filter.Ordering)
                    {
                        case "price_asc": // Сначала дешевые
                            items = items.OrderBy(x => x.Price).ToList();
                            break;
                        case "price_desc": // Сначала дорогие
                            items = items.OrderByDescending(x => x.Price).ToList();
                            break;
                        case "name_asc": // По названию А-Я
                            items = items.OrderBy(x => x.Name).ToList();
                            break;
                        default: // По умолчанию (по ID)
                            items = items.OrderBy(x => x.Id).ToList();
                            break;
                    }
                }

                // 5. Превращаем (маппим) в ViewModel
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
                    Description = $"[GetItemsByFilter] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}