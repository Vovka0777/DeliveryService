using DeliveryService.Domain.ModelsDb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeliveryService.Domain.Models
{
    [Table("userDb")]
    public class UserDb
    {
        [Key]
        public Guid Id { get; set; } // id uuid

        public string? Login { get; set; } // login text
        public string? Password { get; set; } // password text
        public string? Email { get; set; } // email text

        public int Role { get; set; } // role integer (для Role Enum)
        public int? ProfileImg { get; set; }
        public string? PathImage { get; set; }

        [Column("CreatedAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone

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