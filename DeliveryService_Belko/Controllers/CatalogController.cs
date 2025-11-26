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
            // Получаем список товаров из сервиса
            var response = await _itemService.GetItems();

            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                // Передаем список (Data) в представление
                return View(response.Data);
            }

            return RedirectToAction("Error");
        }
    }
}