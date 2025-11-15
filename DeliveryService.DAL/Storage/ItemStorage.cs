using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.DAL.Storage
{
    internal class ItemStorage : IBaseStorage<Item>
    {
        private readonly ApplicationDbContext _context;

        public ItemStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Item item)
        {
            await _context.Items.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Item> Get(Guid item)
        {
            // Ищем предмет по Id.
            return await _context.Items.FindAsync(item);
        }

        public IQueryable<Item> GetAll()
        {
            return _context.Items.AsQueryable();
        }

        public async Task<Item> Update(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}