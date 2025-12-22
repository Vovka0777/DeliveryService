using DeliveryService.Domain.ViewModels.Profile;
using DeliveryService.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
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
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _profileService.UpdateProfile(model);
                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    // --- НАЧАЛО: Обновление аватарки в шапке ---
                    if (User.Identity is ClaimsIdentity currentIdentity)
                    {
                        // Удаляем старый путь
                        var avatarClaim = currentIdentity.FindFirst("AvatarPath");
                        if (avatarClaim != null) currentIdentity.RemoveClaim(avatarClaim);

                        // Добавляем новый путь (берем из response.Data, который мы исправили в Шаге 1)
                        currentIdentity.AddClaim(new Claim("AvatarPath", response.Data.AvatarPath ?? "/images/user.png"));

                        // Перезаписываем куки
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(currentIdentity));
                    }
                    // --- КОНЕЦ ---

                    return RedirectToAction("SiteInformation", "Home");
                }
            }
            return RedirectToAction("SiteInformation", "Home");
        }
    }
}