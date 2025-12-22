using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Service.Interfaces;
using System.Threading.Tasks;

namespace DeliveryService_Belko.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var response = await _orderService.GetCart(User.Identity.Name);
            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return RedirectToAction("Error");
        }

        [HttpPost] // Изменен на POST для безопасности, но можно и GET для теста
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var response = await _orderService.AddToCart(id, User.Identity.Name);
            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                // Возвращаем JSON для AJAX запроса
                return Json(new { description = "Товар добавлен" });
            }
            return Json(new { description = "Ошибка добавления" });
        }

        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            await _orderService.RemoveFromCart(id, User.Identity.Name);
            return RedirectToAction("Cart");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            await _orderService.ConfirmOrder(User.Identity.Name);
            return RedirectToAction("Index", "Home"); // Или на страницу "Спасибо за заказ"
        }
    }
}