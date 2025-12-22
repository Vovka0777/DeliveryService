using DeliveryService.Domain.Models; // Для Basket
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Domain.ModelsDb // Обрати внимание на namespace
{
    [Table("userDb")]
    public class UserDb
    {
        [Key]
        public Guid Id { get; set; }

        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public int Role { get; set; }
        public int? ProfileImg { get; set; }

        // --- НОВЫЕ ПОЛЯ (ОБЯЗАТЕЛЬНО ДОБАВИТЬ) ---
        public string? PathImage { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        // -----------------------------------------

        [Column("CreatedAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        // --- СВЯЗЬ С КОРЗИНОЙ ---
        public Basket? Basket { get; set; }
        // ------------------------

        [InverseProperty("Client")]
        public ICollection<Order> ClientOrders { get; set; } = new List<Order>();

        [InverseProperty("Courier")]
        public ICollection<Order> CourierOrders { get; set; } = new List<Order>();
    }

    public enum RoleDb
    {
        [Display(Name = "Пользователь")]
        Client = 0,
        [Display(Name = "Курьер")]
        Courier = 1,
        [Display(Name = "Админ")]
        Admin = 2
    }
}