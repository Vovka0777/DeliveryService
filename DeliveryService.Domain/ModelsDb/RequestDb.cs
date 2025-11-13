using DeliveryService.Domain.ModelsDb.DeliveryService.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DeliveryService.Domain.Enum.Enum;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("request")]
    public class RequestDb
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_user")]
        public Guid UserId { get; set; } // Внешний ключ к user

        [Column("description")]
        public string Description { get; set; }

        [Column("path_img")]
        public string PathImg { get; set; }

        [Column("status")]
        public Status Status { get; set; } // Используем Enum Status

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        // Навигационное свойство
        [ForeignKey("UserId")]
        public UserDb User { get; set; }
    }
}
