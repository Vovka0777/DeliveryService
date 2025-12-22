using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService_Belko.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _cartService.GetItems(User.Identity.Name);
            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", response.Description);
        }

        public async Task<IActionResult> Add(Guid id)
        {
            await _cartService.AddItem(User.Identity.Name, id);
            return RedirectToAction("Index", "Catalog");
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            await _cartService.RemoveItem(User.Identity.Name, id);
            return RedirectToAction("Index");
        }
    }
}