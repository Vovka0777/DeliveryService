using DeliveryService.Domain.Filters;
using DeliveryService.Domain.ViewModels.Catalog;
using DeliveryService.Domain.ViewModels.Item;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DeliveryService_Belko.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IItemService _itemService;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _appEnvironment;

        public CatalogController(IItemService itemService, Microsoft.AspNetCore.Hosting.IWebHostEnvironment appEnvironment)
        {
            _itemService = itemService;
            _appEnvironment = appEnvironment;
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

        [HttpPost]
        public async Task<IActionResult> GetItemsByFilter([FromBody] ItemFilter filter)
        {
            var response = await _itemService.GetItemsByFilter(filter);
            return Json(response);
        }

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Save(Guid id)
        {
            if (id == Guid.Empty) return View(new ItemViewModel());

            var response = await _itemService.GetItem(id);
            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Save(ItemViewModel model, IFormFile avatar)
        {
            if (model.Price <= 0) ModelState.AddModelError("Price", "Цена не может быть отрицательной");

            if (avatar != null)
            {
                string path = "/images/" + avatar.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileStream);
                }
                model.PathImg = path;
            }

            if (model.Id == Guid.Empty)
            {
                await _itemService.Create(model);
            }
            else
            {
                await _itemService.Edit(model.Id, model);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _itemService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}