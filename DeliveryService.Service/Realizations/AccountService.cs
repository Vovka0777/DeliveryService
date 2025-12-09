    using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Validator;
using System.Security.Claims;
using DeliveryService.Domain.Helpers;
using FluentValidation;
using DeliveryService.Domain.Enum;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting; 
using System;
using System.Linq;

namespace DeliveryService.Service.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly IMapper _mapper;
        private readonly UserValidator _validationRules;
        private readonly IWebHostEnvironment _appEnvironment; // Добавлено

        public AccountService(IBaseStorage<UserDb> userStorage, IMapper mapper, UserValidator validationRules, IWebHostEnvironment appEnvironment) // Добавлено IWebHostEnvironment
        {
            _userStorage = userStorage;
            _mapper = mapper;
            _validationRules = validationRules;
            _appEnvironment = appEnvironment; // Инициализация
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        {
            try
            {
                // Ищем пользователя, у которого введенный текст совпадает либо с Email, либо с Login
                var userDb = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Login || x.Login == model.Login);

                if (userDb == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                if (userDb.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                var user = _mapper.Map<User>(userDb);
                var identity = AuthenticateUserHelper.Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = identity,
                    Description = "Вход выполнен успешно",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<string>> Register(User model)
        {
            try
            {
                Random random = new Random();
                string confirmationCode = $"{random.Next(10)}{random.Next(10)}{random.Next(10)}{random.Next(10)}";

                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) != null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                await SendEmail(model.Email, confirmationCode);

                return new BaseResponse<string>()
                {
                    Data = confirmationCode,
                    Description = "Код подтверждения отправлен на почту",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"Произошла внутренняя ошибка сервера: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task SendEmail(string email, string confirmationCode)
        {
            string path = Path.Combine(_appEnvironment.WebRootPath, "TXT", "password.txt");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Delivery Service", "vovabelko07@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Ваш код подтверждения";

            var builder = new BodyBuilder();

            builder.HtmlBody = $@"
    <div style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;"">
        <div style=""max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 10px; overflow: hidden; box-shadow: 0 2px 5px rgba(0,0,0,0.1);"">
            
            <div style=""background-color: #2c3e50; padding: 20px; text-align: center;"">
                <h1 style=""color: #ffffff; margin: 0; font-size: 24px;"">Delivery Service</h1>
            </div>

            <div style=""padding: 30px; color: #333333;"">
                <h2 style=""margin-top: 0; color: #2c3e50;"">Добро пожаловать!</h2>
                <p style=""font-size: 16px; line-height: 1.5;"">Спасибо за регистрацию. Чтобы подтвердить свой аккаунт, введите этот код в приложении:</p>
                
                <div style=""background-color: #e8f0fe; border: 1px dashed #4a90e2; padding: 15px; text-align: center; font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #2c3e50; margin: 25px 0; border-radius: 8px;"">
                    {confirmationCode}
                </div>

                <p style=""font-size: 14px; color: #7f8c8d;"">Если вы не запрашивали этот код, просто проигнорируйте это письмо.</p>
            </div>

            <div style=""background-color: #ecf0f1; padding: 15px; text-align: center; font-size: 12px; color: #7f8c8d;"">
                &copy; 2025 Delivery Service Belko. Все права защищены.
            </div>
        </div>
    </div>";

            emailMessage.Body = builder.ToMessageBody();

            string password;
            using (StreamReader reader = new StreamReader(path))
            {
                password = (await reader.ReadToEndAsync()).Trim();
            }

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465, true);
                await client.AuthenticateAsync("vovabelko07@mail.ru", password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode)
        {
            try
            {
                if (code != confirmCode)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный код! Регистрация не выполнена.",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                await _validationRules.ValidateAndThrowAsync(model);

                model.Id = Guid.NewGuid();
                model.CreatedAt = DateTime.UtcNow;

                model.PathImage = "/images/user.png";
                model.Role = (int)Role.Client;
                model.ProfileImg = 1;

                model.Password = HashPasswordHelper.HashPassword(model.Password);

                var userDb = _mapper.Map<UserDb>(model);

                await _userStorage.Add(userDb);

                var result = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Пользователь успешно зарегистрирован",
                    StatusCode = StatusCode.OK
                };
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = errorMessage,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                var fullError = ex.Message;
                if (ex.InnerException != null)
                {
                    fullError += $" ---> INNER: {ex.InnerException.Message}";
                }

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"Ошибка БД: {fullError}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
        {
            try
            {
                // Сначала ищем пользователя
                var existingUser = await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email);

                if (existingUser == null)
                {
                    // === ЗАПОЛНЯЕМ ВСЕ ОБЯЗАТЕЛЬНЫЕ ПОЛЯ, КАК ПРИ ОБЫЧНОЙ РЕГИСТРАЦИИ ===

                    // 1. Генерация ID (если база не создает его сама, скорее всего это Guid)
                    model.Id = Guid.NewGuid();

                    // 2. Пароль заглушка
                    model.Password = "google";

                    // 3. Дата создания
                    model.CreatedAt = DateTime.UtcNow;

                    // 4. Роль (обязательно!)
                    model.Role = (int)Role.Client;

                    // 5. Картинка профиля (если есть такая логика в базе, например FK)
                    // В ConfirmEmail у вас стоит: model.ProfileImg = 1;
                    model.ProfileImg = 1;

                    // Если путь к картинке пустой, ставим заглушку
                    if (string.IsNullOrEmpty(model.PathImage))
                    {
                        model.PathImage = "/images/user.png";
                    }

                    // Маппинг в сущность БД
                    var userDb = _mapper.Map<UserDb>(model);

                    // Сохранение
                    await _userStorage.Add(userDb);

                    var resultRegister = AuthenticateUserHelper.Authenticate(model);
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Data = resultRegister,
                        Description = "Объект добавился",
                        StatusCode = StatusCode.OK
                    };
                }

                // Если пользователь уже есть, просто авторизуем его
                // Важно: нужно авторизовать using existingUser (данные из БД), а не model (данные из Google)
                // чтобы подтянулись корректные Role и Id
                var userForAuth = _mapper.Map<User>(existingUser);

                var resultLogin = AuthenticateUserHelper.Authenticate(userForAuth);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultLogin,
                    Description = "Объект уже был создан",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                // ЧТОБЫ УВИДЕТЬ РЕАЛЬНУЮ ПРИЧИНУ ОШИБКИ:
                // База данных прячет ошибку внутри InnerException. Достаем её:
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"Ошибка сохранения: {message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}