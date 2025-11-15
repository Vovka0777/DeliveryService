using AutoMapper;
using DeliveryService.DAL;
using DeliveryService.Domain.Enum; 
using DeliveryService.Domain.Models; 
using DeliveryService.Domain.Response;
using DeliveryService.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; 
using System; 

namespace DeliveryService.Service.Realizations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<User> _userStorage;
        private readonly IMapper _mapper;

        public AccountService(IBaseStorage<User> userStorage, IMapper mapper)
        {
            _userStorage = userStorage;
            _mapper = mapper;
        }
        public async Task<BaseResponse<User>> Login(User model)
        {
            try
            {
                var userDb = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (userDb == null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                if (userDb.Password != model.Password)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Неверный пароль или почта",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                var finalModel = _mapper.Map<User>(userDb);

                return new BaseResponse<User>()
                {
                    Data = finalModel,
                    Description = "Вход выполнен успешно",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<User>> Register(User model)
        {
            try
            {
                var existingUser = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (existingUser != null)
                {
                    return new BaseResponse<User>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                        StatusCode = StatusCode.BadRequest
                    };
                }
                model.ProfileImg = 1;
                model.CreatedAt = DateTime.Now;
                model.Role = (int)Role.Client; // <-- ИСПРАВЛЕНО
                var userDb = _mapper.Map<User>(model); // <-- ИСПРАВЛЕНО
                await _userStorage.Add(userDb);

                return new BaseResponse<User>()
                {
                    Data = model,
                    Description = "Пользователь зарегистрирован",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}