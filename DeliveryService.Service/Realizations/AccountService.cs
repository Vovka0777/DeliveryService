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
using Microsoft.AspNetCore.Hosting; // Добавлено
using System.Net.Http; // Добавлено
using System.IO; // Добавлено
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

        // ... (Login, Register, SendEmail, ConfirmEmail, IsCreatedAccount - методы без изменений)

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
                        StatusCode = StatusCode.NotFound
                    };
                }

                if (userDb.Password != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или почта",
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
            string path = @"E:\инфа\praktika\DeliveryService\DeliveryService_Belko\wwwroot\TXT\password.txt";
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "vovabelko07@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Добро пожаловать!";

            var builder = new BodyBuilder();
            builder.HtmlBody = "<html><body><h1>Ваш код: " + confirmationCode + "</h1></body></html>";
            emailMessage.Body = builder.ToMessageBody();

            string password;
            using (StreamReader reader = new StreamReader(path))
            {
                password = await reader.ReadToEndAsync();
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
                var userDb = new UserDb();
                if ((await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email)) == null)
                {
                    model.Password = "google";
                    model.CreatedAt = DateTime.Now;

                    userDb = _mapper.Map<UserDb>(model);

                    await _userStorage.Add(userDb);

                    var resultRegister = AuthenticateUserHelper.Authenticate(model);
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Data = resultRegister,
                        Description = "Объект добавился",
                        StatusCode = StatusCode.OK
                    };
                }

                var resultLogin = AuthenticateUserHelper.Authenticate(model);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultLogin,
                    Description = "Объект уже был создан",
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
    }
}