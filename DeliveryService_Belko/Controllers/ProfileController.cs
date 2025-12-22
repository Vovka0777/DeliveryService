using DeliveryService.Domain.ViewModels.Profile;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryService_Belko.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _profileService.GetProfile(User.Identity.Name);
            if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            // Исправлен редирект (был Index, стало SiteInformation)
            return RedirectToAction("SiteInformation", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _profileService.UpdateProfile(model);
                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    // --- ОБНОВЛЕНИЕ КУКИ (LOGIN + AVATAR) ---
                    if (User.Identity is ClaimsIdentity currentIdentity)
                    {
                        // 1. Обновляем путь к аватарке
                        var avatarClaim = currentIdentity.FindFirst("AvatarPath");
                        if (avatarClaim != null) currentIdentity.RemoveClaim(avatarClaim);
                        currentIdentity.AddClaim(new Claim("AvatarPath", response.Data.AvatarPath ?? "/images/user.png"));

                        // 2. Обновляем Логин (если он изменился)
                        if (response.Data.Login != currentIdentity.Name)
                        {
                            var nameClaim = currentIdentity.FindFirst(ClaimTypes.Name);
                            if (nameClaim != null) currentIdentity.RemoveClaim(nameClaim);
                            currentIdentity.AddClaim(new Claim(ClaimTypes.Name, response.Data.Login));
                        }

                        // Перезаписываем куки
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(currentIdentity));
                    }
                    // ----------------------------------------

                    return RedirectToAction("Index");
                }
                else
                {
                    // Если ошибка (например, пароль неверный), показываем её на странице
                    ModelState.AddModelError("", response.Description);
                }
            }
            // Если валидация не прошла, возвращаем View с ошибками
            return View("Index", model);
        }
    }
}