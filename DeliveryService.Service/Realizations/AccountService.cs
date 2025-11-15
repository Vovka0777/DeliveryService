using AutoMapper;
using DeliveryService.Service.Interfaces;
using DeliveryService.Domain.Models; 
using DeliveryService.DAL;              
using DeliveryService.Domain.Response;

namespace DeliveryService.Service.Realizations
{
    public class AccountService : IAccountService 
    {
        private readonly IBaseStorage<User> _userStorage; 
        private readonly IMapper _mapper; 
        public AccountService(IBaseStorage<User> userStorage, IMapper mapper) //
        {
            _userStorage = userStorage;
            _mapper = mapper;
        }
        public Task<BaseResponse<User>> Login(User userModel) 
        {
            throw new System.NotImplementedException();
        }
        public Task<BaseResponse<User>> Register(User userModel) 
        {
            throw new System.NotImplementedException();
        }
    }
}