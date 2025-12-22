using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryService.Domain.Enum;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("request")]
    public class RequestDb
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_user")]
        public Guid UserId { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("path_img")]
        public string PathImg { get; set; } = string.Empty;

        [Column("status")]
        public StatusOrder Status { get; set; } // Исправлено: Status -> StatusOrder

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public UserDb User { get; set; } = null!;
    }
}