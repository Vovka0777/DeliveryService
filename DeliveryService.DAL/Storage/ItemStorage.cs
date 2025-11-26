using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryService.DAL.Storage
{
    public class ItemStorage : IBaseStorage<Item>
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
            return _context.Items.Select(x => new Item
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Category = x.Category,
                PathImg = x.PathImg,
                CreatedAt = x.CreatedAt
            });
        }

        public async Task<Item> Update(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}