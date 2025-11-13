using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("item")]
    public class ItemDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_order")]
        public Guid OrderId { get; set; } // Внешний ключ к orders

        [Column("name")]
        public string Name { get; set; }

        [Column("path_img")]
        public string PathImg { get; set; }

        // Навигационное свойство
        [ForeignKey("OrderId")]
        public OrderDb Order { get; set; }
    }
}
