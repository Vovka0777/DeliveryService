using DeliveryService.Domain.Filters; // Подключаем фильтры
using DeliveryService.Domain.ViewModels.Catalog;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
                var model = new CatalogViewModel() { Items = response.Data };
                return View(model);
            }
            return View("Error", $"{response.Description}");
        }

        // === НОВЫЙ МЕТОД (для AJAX запроса) ===
        [HttpPost]
        public async Task<IActionResult> GetItemsByFilter([FromBody] ItemFilter filter)
        {
            var response = await _itemService.GetItemsByFilter(filter);

            // Возвращаем результат в формате JSON, чтобы JS мог его прочитать
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetItem(Guid id)
        {
            return View();
        }
    }
}