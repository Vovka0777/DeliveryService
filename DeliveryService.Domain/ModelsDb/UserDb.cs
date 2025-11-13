using DeliveryService.Domain.ModelsDb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeliveryService.Domain.Models
{
    // Аналогично Рисунку 119
    [Table("user")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } // id uuid

        public string? Login { get; set; } // login text
        public string? Password { get; set; } // password text
        public string? Email { get; set; } // email text

        public int Role { get; set; } // role integer (для Role Enum)
        public int? ProfileImg { get; set; } // profile_img integer (Сделаем nullable, если это id, которое может отсутствовать)

        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone

        // Навигационные свойства
        [InverseProperty("Client")]
        public ICollection<Order> ClientOrders { get; set; } = new List<Order>();

        [InverseProperty("Courier")]
        public ICollection<Order> CourierOrders { get; set; } = new List<Order>();
    }

    // Пример Enum'а для ролей (Аналогично Рисунку 120)
    public enum Role
    {
        [Display(Name = "Пользователь")]
        Client = 0,
        [Display(Name = "Курьер")]
        Courier = 1,
        [Display(Name = "Админ")]
        Admin = 2
    }
}