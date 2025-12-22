using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DeliveryService.Service.Interfaces;
using DeliveryService.Domain.ViewModels.Profile;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using DeliveryService.Domain.Models; // Для User, если нужно

namespace DeliveryService_Belko.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        // В идеале вынести в IAccountService метод GetProfile и UpdateProfile
        // Здесь упрощенно для примера:
        private readonly IAccountService _accountService; // Предполагается, что он есть
        private readonly IWebHostEnvironment _appEnvironment;

        public ProfileController(IAccountService accountService, IWebHostEnvironment appEnvironment)
        {
            _accountService = accountService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Здесь должна быть логика получения профиля из сервиса
            // var response = await _accountService.GetProfile(User.Identity.Name);
            // Пока заглушка:
            var model = new ProfileViewModel { Name = User.Identity.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Avatar != null)
                {
                    // Логика сохранения аватарки
                    string path = "/ImageUser/" + model.Avatar.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.Avatar.CopyToAsync(fileStream);
                    }
                    model.AvatarPath = path;
                }

                // Вызов сервиса обновления: await _accountService.UpdateProfile(model);
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }
    }
}