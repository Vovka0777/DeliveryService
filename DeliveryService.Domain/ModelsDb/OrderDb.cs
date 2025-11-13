using DeliveryService.Domain.ModelsDb.DeliveryService.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("orders")]
    public class OrderDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_user")]
        public Guid UserId { get; set; } // Внешний ключ: Заказчик

        [Column("id_courier")]
        public Guid? CourierId { get; set; } // Внешний ключ: Курьер (может быть NULL)

        [Column("name")]
        public string Name { get; set; } // Название заказа/краткое описание

        [Column("price")]
        public decimal Price { get; set; }

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        // Навигационные свойства
        [ForeignKey("UserId")]
        public UserDb User { get; set; } // Заказчик

        [ForeignKey("CourierId")]
        public UserDb Courier { get; set; } // Курьер

        public ICollection<ItemDb> Items { get; set; } // Товары в заказе
        public ICollection<OrderHistoryDb> History { get; set; } // История статусов
    }
}
