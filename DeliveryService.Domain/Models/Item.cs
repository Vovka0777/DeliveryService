namespace DeliveryService.Domain.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public Guid IdOrder { get; set; } 
        public Order Order { get; set; } = null!;
        public string? Name { get; set; }
        public string? PathImg { get; set; }
    }
}