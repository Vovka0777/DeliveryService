
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeliveryService.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; } // id uuid
        public string? Login { get; set; } // login text
        public string? Password { get; set; } // password text
        public string? Email { get; set; } // email text
        public int Role { get; set; } // role integer (для Role Enum)
        public int? ProfileImg { get; set; } // profile_img integer (Сделаем nullable, если это id, которое может отсутствовать)
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone
        public ICollection<Order> ClientOrders { get; set; } = new List<Order>();
        public ICollection<Order> CourierOrders { get; set; } = new List<Order>();
    }
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