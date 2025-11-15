using static DeliveryService.Domain.Enum.Enum;

namespace DeliveryService.Domain.Models
{
    public class Request
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Внешний ключ к user
        public string Description { get; set; }
        public string PathImg { get; set; }       
        public Status Status { get; set; } // Используем Enum Status
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
    }
}
