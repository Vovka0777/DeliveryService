using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DeliveryService.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DeliveryService.DAL
{
    public class ApplicationDbContext : DbContext
    {
        // Конструктор, аналогичный Рисунку 117
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Добавляем DbSet'ы для ваших таблиц
        // Предполагается, что ваши модели называются: User, Order, OrderHistory, Item
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<Item> Items { get; set; }

        protected readonly IConfiguration Configuration;

        // Если нужен конструктор для миграций:
        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
