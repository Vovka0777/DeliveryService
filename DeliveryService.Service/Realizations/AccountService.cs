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

namespace DeliveryService.Service.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly IMapper _mapper;
        private readonly UserValidator _validationRules;

        public AccountService(IBaseStorage<UserDb> userStorage, IMapper mapper, UserValidator validationRules)
        {
            _userStorage = userStorage;
            _mapper = mapper;
            _validationRules = validationRules;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);

                var userDb = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (userDb == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                        // Убедитесь, что вы используете enum Role, а не int, при обращении к Role.
                        StatusCode = (DeliveryService.Domain.Enum.StatusCode.NotFound)
                    };
                }

                if (userDb.Password != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или почта",
                        StatusCode = (DeliveryService.Domain.Enum.StatusCode.BadRequest)
                    };
                }

                var user = _mapper.Map<User>(userDb);
                // 1. Создаем ClaimsIdentity для аутентификации
                var identity = AuthenticateUserHelper.Authenticate(user);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = identity, // ⬅️ ИСПРАВЛЕНО: Возвращаем ClaimsIdentity
                    Description = "Вход выполнен успешно",
                    StatusCode = (DeliveryService.Domain.Enum.StatusCode.OK)
                };
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.BadRequest
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
                        StatusCode = StatusCode.BadRequest // Добавим корректный статус
                    };
                }

                await SendEmail(model.Email, confirmationCode);

                return new BaseResponse<string>()
                {
                    Data = confirmationCode,
                    Description = "Код подтверждения отправлен на почту. Проверьте ваш email.",
                    StatusCode = StatusCode.OK
                };
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errorMessage = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<String>()
                {
                    Description = errorMessage, // Используем список ошибок валидации
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex) // ⬅️ Общий блок для обработки ошибок, включая SendEmail
            {
                // Логирование ошибки здесь было бы очень полезно!
                Console.WriteLine($"Ошибка при регистрации или отправке почты: {ex.Message}");
                // Возвращаем информативный ответ с ошибкой
                return new BaseResponse<string>()
                {
                    Description = $"Произошла внутренняя ошибка сервера: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError // 500
                };
            }
        }

        public async Task SendEmail(string email, string confirmationCode)
    {
        string path = @"E:\инфа\praktika\DeliveryService\DeliveryService_Belko\wwwroot\TXT\password.txt";
        var emailMessage = new MimeMessage();

            // Добавление отправителя
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "vovabelko07@mail.ru"));

            // Добавление получателя
            emailMessage.To.Add(new MailboxAddress("", email));

        // Тема письма
        emailMessage.Subject = "Добро пожаловать!";

        // Тело письма в формате HTML
        var builder = new BodyBuilder();
        builder.HtmlBody =
            "<html>" +
                "<head>" +
                    "<style>" +
                        "* { font-family: Arial, sans-serif; background-color: #f2f2f2; }" +
                        ".container { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 8px 10px rgba(0,0,0,0.1); }" +
                        ".header { text-align: center; margin-bottom: 20px; }" +
                        ".message { font-size: 16px; line-height: 1.6; }" +
                        ".container-code { background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }" +
                        ".code { text-align: center; }" +
                    "</style>" +
                "</head>" +
                "<body>" +
                    "<div class=\"container\">" +
                        "<div class=\"header\"><h1>Добро пожаловать на сайт Службы доставки Брэгд!</h1></div>" +
                        "<div class=\"message\">" +
                            "<p>Пожалуйста, введите данный код на сайте, чтобы подтвердить ваш email и завершить регистрацию:</p>" +
                        "</div>" +
                        "<div class=\"container-code\"><p class=\"code\">" + confirmationCode + "</p></div>" +
                    "</div>" +
                "</body>" +
            "</html>";

        emailMessage.Body = builder.ToMessageBody();

        // Чтение пароля из файла
        string password;
        using (StreamReader reader = new StreamReader(path))
        {
            password = await reader.ReadToEndAsync();
        }

        // Отправка письма
        using (var client = new SmtpClient())
        {
            // Подключение к SMTP-серверу Gmail
            await client.ConnectAsync("smtp.mail.ru", 465, true);

            // Аутентификация
            await client.AuthenticateAsync("vovabelko07@mail.ru", password);

            // Отправка
            await client.SendAsync(emailMessage);

            // Отключение
            await client.DisconnectAsync(true);
        }
    }
        public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode)
        {
            try
            {
                // Проверка кода подтверждения
                if (code != confirmCode)
                {
                    throw new Exception("Неверный код! Регистрация не выполнена.");
                }

                // Инициализация полей модели
                model.PathImage = "/images/user.png";
                model.CreatedAt = DateTime.Now;
                // Хеширование пароля
                model.Password = HashPasswordHelper.HashPassword(model.Password);

                // Применение правил валидации
                await _validationRules.ValidateAndThrowAsync(model);

                // Маппинг модели в модель базы данных
                var userDb = _mapper.Map<UserDb>(model);

                // Добавление пользователя в хранилище (базу данных)
                await _userStorage.Add(userDb);

                // Аутентификация пользователя
                var result = AuthenticateUserHelper.Authenticate(model);

                // Возврат успешного ответа
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился", // Вероятно, здесь имелось в виду "Пользователь зарегистрирован"
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
    }
}