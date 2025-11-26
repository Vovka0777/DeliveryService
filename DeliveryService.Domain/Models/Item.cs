namespace DeliveryService.Domain.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Category { get; set; }
        public string PathImg { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}