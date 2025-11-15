namespace DeliveryService.Domain.Models
{
    public class OrderHistory
    {
        public Guid Id { get; set; } // id uuid
        public Guid IdOrder { get; set; } // id_order uuid
        public Order Order { get; set; } = null!;
        public Guid IdStatus { get; set; } // id_status uuid (Внешний ключ к статусу)
        public string? Description { get; set; } // description text
        public string? Location { get; set; } // location text
        public DateTime CreatedAt { get; set; } // createdAt timestamp without time zone
    }
}