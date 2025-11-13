using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Models
{
    [Table("item")]
    public class Item
    {
        [Key]
        public Guid Id { get; set; } // id uuid

        // Связь с Заказом
        public Guid IdOrder { get; set; } // id_order uuid
        [ForeignKey("IdOrder")]
        public Order Order { get; set; } = null!;

        public string? Name { get; set; } // name text
        public string? PathImg { get; set; } // path_img text
    }
}