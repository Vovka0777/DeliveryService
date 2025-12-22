namespace DeliveryService.Domain.Models
{
    public class BasketItem
    {
        public Guid Id { get; set; }

        public Guid BasketId { get; set; }
        public Basket Basket { get; set; }

        public Guid ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
    }
}