using DeliveryService.Domain.ViewModels.LoginAndRegistration;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryService.Service.Interfaces;
using DeliveryService.Domain.Enum;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication; 
using System.Security.Claims; 
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;

namespace DeliveryService_Belko.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IAccountService accountService, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _mapper = mapper;
            _accountService = accountService;
            _appEnvironment = appEnvironment;
        }

        public IActionResult SiteInformation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var response = await _accountService.Login(user);

                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(response.Data));

                    return Ok(model);
                }

                ModelState.AddModelError("", response.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();
            return BadRequest(errors);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var response = await _accountService.Register(user); 

                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {   
                    return Ok(response); 
                }
                ModelState.AddModelError("", response.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();
            return BadRequest(errors);
        }
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SiteInformation", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Login = model.Login,
                    Role = (int)DeliveryService.Domain.Models.Role.Client
                    // Остальные поля заполнятся в сервисе (Id, CreatedAt, PathImage)
                };

                // 2. Вызываем сервис
                var response = await _accountService.ConfirmEmail(user, model.Code, model.ConfirmCode);

                if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                {
                    try
                    {
                        var principal = new ClaimsPrincipal(response.Data);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        // ✅ Возвращаем ПРОСТОЙ JSON без ClaimsIdentity!
                        return Ok(new { success = true, message = "Пользователь успешно зарегистрирован" });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при попытке авторизовать пользователя после регистрации.");
                        return StatusCode(500, new { success = false, description = $"Критическая ошибка авторизации: {ex.Message}" });
                    }
                }

                // Если ошибка в логике сервиса (неверный код и т.д.)
                return BadRequest(new { description = response.Description });
            }

            // Если ошибка валидации модели (пустые поля)
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(errors);
        }
        public async Task<string> SaveImageInImageUser(string imageUrl, AuthenticateResult result)
        {
            string filePath = "";
            if (!string.IsNullOrEmpty(imageUrl))
            {
                using (var httpClient = new HttpClient())
                {
                    filePath = Path.Combine("ImageUser", $"{result.Principal.FindFirst(ClaimTypes.Email)?.Value}-avatar.jpg");

                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                    await System.IO.File.WriteAllBytesAsync(Path.Combine(_appEnvironment.WebRootPath, filePath), imageBytes);
                }
            }
            return filePath;
        }
        public async Task AuthenticationGoogle(string returnUrl = "/*") // По умолчанию возвращаемся на главную
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse", new { returnUrl }), // Передаем returnUrl
                    Parameters = { { "prompt", "select_account" } }
                });
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/*")
        {
            try
            {
                // ИСПРАВЛЕНИЕ: Читаем данные из схемы Google, а не из Куки
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (result?.Succeeded == true)
                {
                    User model = new User
                    {
                        Login = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                        Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,
                        // Добавляем сохранение картинки
                        PathImage = "/" + await SaveImageInImageUser(result.Principal.FindFirst("picture")?.Value, result)
                    };

                    // Если сохранение картинки вернуло пустую строку или null, ставим дефолт (немного поправил логику объединения путей)
                    if (string.IsNullOrEmpty(model.PathImage) || model.PathImage == "/")
                    {
                        model.PathImage = "/images/user.png";
                    }

                    // ВАЖНО: При создании через Google нужно явно указать роль, 
                    // иначе в IsCreatedAccount она может записаться как 0 (если в модели default(int))
                    model.Role = (int)DeliveryService.Domain.Models.Role.Client;

                    var response = await _accountService.IsCreatedAccount(model);

                    if (response.StatusCode == DeliveryService.Domain.Enum.StatusCode.OK)
                    {
                        // А вот здесь мы уже создаем НАШУ куку, чтобы пользователь был залогинен на сайте
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(response.Data));

                        return Redirect(returnUrl);
                    }

                    return BadRequest($"Ошибка сервиса: {response.Description}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest("Аутентификация Google не удалась (result.Succeeded == false).");
        }
    }
}