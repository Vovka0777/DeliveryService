using DeliveryService.Domain.Filters;
using DeliveryService.Domain.ViewModels.Catalog;
using DeliveryService.Domain.ViewModels.Item;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DeliveryService_Belko.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IItemService _itemService;

        public CatalogController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _itemService.GetItems();

            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                // Оборачиваем список товаров в ViewModel каталога
                var model = new CatalogViewModel()
                {
                    Items = response.Data
                };

                return View(model);
            }

            return View("Error", $"{response.Description}");
        }

        // Метод для фильтрации (вызывается через fetch из catalog.js)
        [HttpPost]
        public async Task<IActionResult> GetItemsByFilter([FromBody] ItemFilter filter)
        {
            var response = await _itemService.GetItemsByFilter(filter);

            // Возвращаем JSON с обновленным списком товаров
            return Json(response);
        }

        // Метод для страницы конкретного товара
        [HttpGet]
        public async Task<IActionResult> GetItem(Guid id)
        {
            var response = await _itemService.GetItem(id);

            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }

            return RedirectToAction("Index");
        }
    }
}