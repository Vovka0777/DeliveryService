using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Models
{
    [Table("item")]
    public class ItemsDb
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IdOrder { get; set; } 
        [ForeignKey("IdOrder")]
        public Order Order { get; set; } = null!;

        public string? Name { get; set; }
        public string? PathImg { get; set; }
    }
}