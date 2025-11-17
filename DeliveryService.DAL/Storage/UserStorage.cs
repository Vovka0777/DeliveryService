using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.DAL.Storage
{
    public class UserStorage : IBaseStorage<User>
    {
        private readonly ApplicationDbContext _context;

        public UserStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(User item)
        {
            await _context.Users.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(User item)
        {
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Get(Guid item)
        {
            // Ищем пользователя по Id.
            // FindAsync - это эффективный способ получить сущность по ее первичному ключу.
            return await _context.Users.FindAsync(item);
        }

        public IQueryable<User> GetAll()
        {
            // Возвращаем IQueryable, чтобы можно было строить
            // запросы (например, .Where(), .OrderBy()) в сервисном слое.
            return _context.Users.AsQueryable();
        }

        public async Task<User> Update(User item)
        {
            _context.Users.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}