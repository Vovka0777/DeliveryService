using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DeliveryService.Domain.Enum.Enum;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("order_history")]
    public class OrderHistoryDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_order")]
        public Guid OrderId { get; set; } // Внешний ключ к orders

        [Column("id_status")]
        public Status StatusId { get; set; } // Используем Enum Status

        [Column("description")]
        public string Description { get; set; }

        [Column("location")]
        public string Location { get; set; } // Местоположение в момент смены статуса

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        // Навигационное свойство
        [ForeignKey("OrderId")]
        public OrderDb Order { get; set; }
    }
}
