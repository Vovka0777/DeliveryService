using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Domain.ModelsDb
{
    // Указываем, что этот класс привязан к таблице "Products" в PostgreSQL
    [Table("Products")]
    public class ItemDb
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Category { get; set; } // 0-Еда, 1-Одежда и т.д.

        public string PathImg { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}