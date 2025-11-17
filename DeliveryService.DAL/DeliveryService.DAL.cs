using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Models;

namespace DeliveryService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserDb> Users { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderHistory> OrderHistories { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
    }
}