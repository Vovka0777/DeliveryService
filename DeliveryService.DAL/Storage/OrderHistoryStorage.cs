using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.DAL.Storage
{
    internal class OrderHistoryStorage : IBaseStorage<OrderHistory>
    {
        private readonly ApplicationDbContext _context;

        public OrderHistoryStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(OrderHistory item)
        {
            await _context.OrderHistories.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(OrderHistory item)
        {
            _context.OrderHistories.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderHistory> Get(Guid item)
        {
            // Ищем запись истории по Id.
            return await _context.OrderHistories.FindAsync(item);
        }

        public IQueryable<OrderHistory> GetAll()
        {
            return _context.OrderHistories.AsQueryable();
        }

        public async Task<OrderHistory> Update(OrderHistory item)
        {
            _context.OrderHistories.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}