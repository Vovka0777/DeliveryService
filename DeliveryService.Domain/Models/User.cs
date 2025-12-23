using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryService.Domain.Enum;

namespace DeliveryService.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; } // id uuid
        public string? Login { get; set; } // login text
        public string? Password { get; set; } // password text
        public string? Email { get; set; } // email text
        public Role Role { get; set; }
        public int? ProfileImg { get; set; }
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone
        public string Phone { get; set; }


        public string PathImage { get; set; } = string.Empty;

        // Связь с корзиной
        public Basket? Basket { get; set; }

        public ICollection<Order> ClientOrders { get; set; } = new List<Order>();
        public ICollection<Order> CourierOrders { get; set; } = new List<Order>();
    }
}