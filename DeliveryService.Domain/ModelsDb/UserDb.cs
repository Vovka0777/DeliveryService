using System.ComponentModel.DataAnnotations.Schema;
using DeliveryService.Domain.Enum;

namespace DeliveryService.Domain.ModelsDb
{
    namespace DeliveryService.Domain.ModelsDb
    {
        [Table("user")] // Имя таблицы из вашей ER-диаграммы
        public class UserDb
        {
            [Key]
            [Column("id", TypeName = "uuid")] // Указываем тип UUID, если используется PostgreSQL
            public Guid Id { get; set; }

            [Column("login")]
            public string Login { get; set; }

            [Column("password")]
            public string Password { get; set; }

            [Column("email")]
            public string Email { get; set; }

            [Column("role")]
            public int Role { get; set; } // Или тип Enum Role, как в примере с агентством

            [Column("profile_img")]
            public string ProfileImg { get; set; }

            [Column("createdat", TypeName = "timestamp with time zone")]
            public DateTime CreatedAt { get; set; }

            // Связи: Один-ко-многим (один пользователь может иметь много заказов)
            public ICollection<OrderDb> Orders { get; set; } = new List<OrderDb>();
        }
    }
}
