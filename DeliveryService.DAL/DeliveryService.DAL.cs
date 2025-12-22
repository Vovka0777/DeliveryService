using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Models;
using DeliveryService.Domain.ModelsDb; // Добавил namespace, так как UserDb скорее всего там

namespace DeliveryService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Существующие таблицы
        public DbSet<UserDb> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;

        // --- НОВЫЕ ТАБЛИЦЫ ДЛЯ КОРЗИНЫ ---
        public DbSet<Basket> Baskets { get; set; } = null!;
        public DbSet<BasketItem> BasketItems { get; set; } = null!;
    }
}