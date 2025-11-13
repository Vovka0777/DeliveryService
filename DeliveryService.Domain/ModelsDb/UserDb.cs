using DeliveryService.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static DeliveryService.Domain.Enum.Enum;

namespace DeliveryService.Domain.ModelsDb
{
    namespace DeliveryService.Domain.ModelsDb
    {
        [Table("user")]
        public class UserDb
        {
            [Column("id")]
            public Guid Id { get; set; }

            [Column("login")]
            public string Login { get; set; }

            [Column("password")]
            public string Password { get; set; }

            [Column("email")]
            public string Email { get; set; }

            [Column("role")]
            public Role Role { get; set; } // Используем Enum Role

            [Column("profile_img")]
            public string ProfileImg { get; set; }

            [Column("createdAt", TypeName = "timestamp")]
            public DateTime CreatedAt { get; set; }

            // Навигационные свойства для связей (связь "один ко многим")
            public ICollection<OrderDb> Orders { get; set; } // Заказы, созданные пользователем
            public ICollection<OrderDb> CourierOrders { get; set; } // Заказы, назначенные курьеру
            public ICollection<RequestDb> Requests { get; set; } // Запросы, созданные пользователем
        }
    }
}
