using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                return View(response.Data);
            }

            // ВРЕМЕННО: Выводим текст ошибки на экран, чтобы прочитать его
            return Content($"Ошибка: {response.Description} (Статус: {response.StatusCode})");
        }
    }
}