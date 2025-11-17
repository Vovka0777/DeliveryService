using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.DAL.Storage
{
    public class OrderStorage : IBaseStorage<Order>
    {
        private readonly ApplicationDbContext _context;

        public OrderStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Order item)
        {
            await _context.Orders.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order item)
        {
            _context.Orders.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> Get(Guid item)
        {
            // Ищем заказ по Id.
            return await _context.Orders.FindAsync(item);
        }

        public IQueryable<Order> GetAll()
        {
            return _context.Orders.AsQueryable();
        }

        public async Task<Order> Update(Order item)
        {
            _context.Orders.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}