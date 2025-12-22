using DeliveryService.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DeliveryService.Domain.Enum.Enum;

namespace DeliveryService.Domain.ModelsDb
{
    [Table("request")]
    public class RequestDb
    {
        [Key] // Добавлен атрибут Key
        [Column("id")]
        public Guid Id { get; set; }

        [Column("id_user")]
        public Guid UserId { get; set; } // Внешний ключ к user

        [Column("description")]
        public string Description { get; set; } = string.Empty; // Инициализация для предотвращения null-предупреждений

        [Column("path_img")]
        public string PathImg { get; set; } = string.Empty;

        [Column("status")]
        public Status Status { get; set; } // Используем Enum Status

        [Column("createdAt", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public UserDb User { get; set; } = null!; // Изменено на UserDb для согласованности с UserDb.cs
    }
}