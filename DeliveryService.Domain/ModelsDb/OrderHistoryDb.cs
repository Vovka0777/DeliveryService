using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Models
{
    [Table("order_history")]
    public class OrderHistoryDb
    {
        [Key]
        public Guid Id { get; set; } // id uuid

        // Связь с Заказом
        public Guid IdOrder { get; set; } // id_order uuid
        [ForeignKey("IdOrder")]
        public Order Order { get; set; } = null!;

        public Guid IdStatus { get; set; } // id_status uuid (Внешний ключ к статусу)

        public string? Description { get; set; } // description text
        public string? Location { get; set; } // location text

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone
    }
}