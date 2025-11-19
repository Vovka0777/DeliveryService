using DeliveryService.Domain.ModelsDb;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Domain.Models
{
    [Table("orders")]
    public class OrderDb
    {
        [Key]
        public Guid Id { get; set; } // id uuid

        public Guid IdUser { get; set; } // id_user uuid
        [ForeignKey("IdUser")]
        public User Client { get; set; } = null!; // Навигационное свойство. null! используется для свойств, которые EF Core заполнит.

        public Guid? IdCourier { get; set; } // id_courier uuid
        [ForeignKey("IdCourier")]
        public User? Courier { get; set; }

        public string? Name { get; set; } // name text (Например, имя получателя)
        public decimal Price { get; set; } // price numeric

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone

        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<OrderHistory> History { get; set; } = new List<OrderHistory>();
    }
}