using DeliveryService.Domain;
using DeliveryService.Domain.ModelsDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata; // 👈 Добавлено для DeleteBehavior
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic; // 👈 Добавлено для коллекций (если они используются в моделях)

namespace DeliveryService.DAL // Замените на ваш namespace
{
    public class ApplicationDbContext : DbContext
    {
        // DbSet'ы инициализированы, чтобы избежать предупреждений Nullability

            
        public DbSet<UserDb> UsersDb { get; set; } = null!;
        public DbSet<OrderDb> OrdersDb { get; set; } = null!;
        public DbSet<ItemDb> ItemsDb { get; set; } = null!;
        public DbSet<OrderHistoryDb> OrderHistoriesDb { get; set; } = null!;
        public DbSet<RequestDb> RequestsDb { get; set; } = null!;

        // Сделано допускающим NULL (IConfiguration?) и убрано readonly,
        // чтобы избежать ошибок, если конфигурация не передана.
        private IConfiguration? Configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Конструктор с IConfiguration
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Указываем EF Core, что id_courier и id_user в orders ссылаются на UserDb
            modelBuilder.Entity<OrderDb>()
        .HasOne(o => o.User)
        .WithMany(u => u.Orders)
        .HasForeignKey(o => o.UserId)
        .OnDelete(DeleteBehavior.Restrict); // Заказчик

            modelBuilder.Entity<OrderDb>()
        .HasOne(o => o.Courier)
        .WithMany(u => u.CourierOrders)
        .HasForeignKey(o => o.CourierId)
        .IsRequired(false) // id_courier может быть null
                .OnDelete(DeleteBehavior.SetNull); // При удалении курьера, Orders.CourierId = null

            // Настройка связей один-ко-многим для OrderHistory, Item, Request:

            modelBuilder.Entity<ItemDb>()
        .HasOne(i => i.Order)
        .WithMany(o => o.Items)
        .HasForeignKey(i => i.OrderId);

            modelBuilder.Entity<OrderHistoryDb>()
              .HasOne(h => h.Order)
              .WithMany(o => o.History)
              .HasForeignKey(h => h.OrderId);

            modelBuilder.Entity<RequestDb>()
              .HasOne(r => r.User)
              .WithMany(u => u.Requests)
              .HasForeignKey(r => r.UserId);

            // Преобразование Enum в int
            modelBuilder.Entity<UserDb>()
        .Property(u => u.Role)
        .HasConversion<int>();

            modelBuilder.Entity<RequestDb>()
              .Property(r => r.Status)
              .HasConversion<int>();

            modelBuilder.Entity<OrderHistoryDb>()
              .Property(h => h.StatusId)
              .HasConversion<int>();

            base.OnModelCreating(modelBuilder);
        }
    }
}