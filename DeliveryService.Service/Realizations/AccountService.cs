using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.Response;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using DeliveryService.Domain.Helpers; // ⬅️ Добавлен using для AuthenticateUserHelper

namespace DeliveryService.Service.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly IMapper _mapper;

        public AccountService(IBaseStorage<UserDb> userStorage, IMapper mapper)
        {
            _userStorage = userStorage;
            _mapper = mapper;
        }

        public async Task<BaseResponse<ClaimsIdentity>> Login(User model)
        {
            try
            {
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
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = (DeliveryService.Domain.Enum.StatusCode.InternalServerError)
                };
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> Register(User model)
        {
            try
            {
                var existingUser = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (existingUser != null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                        StatusCode = (DeliveryService.Domain.Enum.StatusCode)DeliveryService.Domain.Models.Role.Client
                    };
                }

                // Назначаем стандартные значения
                model.ProfileImg = 1;
                model.CreatedAt = DateTime.Now;
                model.Role = (int)DeliveryService.Domain.Models.Role.Client;

                // Маппинг и сохранение
                var userDb = _mapper.Map<UserDb>(model);

                await _userStorage.Add(userDb);

                // 2. Создаем ClaimsIdentity после успешной регистрации
                var identity = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = identity, // ⬅️ ИСПРАВЛЕНО: Возвращаем ClaimsIdentity
                    Description = "Пользователь зарегистрирован",
                    StatusCode = (DeliveryService.Domain.Enum.StatusCode)DeliveryService.Domain.Models.Role.Client
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = (DeliveryService.Domain.Enum.StatusCode)DeliveryService.Domain.Models.Role.Client
                };
            }
        }
    }
}