using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Models;
using Microsoft.Extensions.Configuration; // Эту директиву можно удалить, если IConfiguration больше не используется

namespace DeliveryService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        // Мы удаляем protected readonly IConfiguration? Configuration;

        // ОСТАВЛЯЕМ ТОЛЬКО ЭТОТ КОНСТРУКТОР
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Удаляем конструктор public ApplicationDbContext(IConfiguration configuration) { ... }

        // DbSet'ы для ваших таблиц
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
    }
}