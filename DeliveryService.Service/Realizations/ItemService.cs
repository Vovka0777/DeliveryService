using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Filters;
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

        // Метод для получения всех товаров (используется в Index по умолчанию)
        public async Task<BaseResponse<List<ItemViewModel>>> GetItems()
        {
            try
            {
                var items = await _itemStorage.GetAll().ToListAsync();
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

        // Метод для фильтрации и сортировки
        public async Task<BaseResponse<List<ItemViewModel>>> GetItemsByFilter(ItemFilter filter)
        {
            try
            {
                var items = await _itemStorage.GetAll().ToListAsync();

                // 1. Фильтрация по Цене
                if (filter.MaxPrice > 0)
                {
                    items = items.Where(x => x.Price <= filter.MaxPrice).ToList();
                }

                // 2. Фильтрация по Категориям
                if (filter.Categories != null && filter.Categories.Any())
                {
                    items = items.Where(x => filter.Categories.Contains((int)x.Category)).ToList();
                }

                // 3. НОВОЕ: Фильтрация по Названию (Поиск)
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    // Приводим к нижнему регистру для поиска без учета регистра
                    items = items.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower())).ToList();
                }

                // 4. Сортировка (без изменений)
                if (!string.IsNullOrEmpty(filter.Ordering))
                {
                    switch (filter.Ordering)
                    {
                        case "price_asc":
                            items = items.OrderBy(x => x.Price).ToList();
                            break;
                        case "price_desc":
                            items = items.OrderByDescending(x => x.Price).ToList();
                            break;
                        case "name_asc":
                            items = items.OrderBy(x => x.Name).ToList();
                            break;
                        default:
                            items = items.OrderBy(x => x.Id).ToList();
                            break;
                    }
                }

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

        // Метод для получения одного товара по ID
        public async Task<BaseResponse<ItemViewModel>> GetItem(Guid id)
        {
            try
            {
                var item = await _itemStorage.Get(id);

                if (item == null)
                {
                    return new BaseResponse<ItemViewModel>()
                    {
                        Description = "Товар не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                var result = _mapper.Map<ItemViewModel>(item);

                return new BaseResponse<ItemViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ItemViewModel>()
                {
                    Description = $"[GetItem] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}