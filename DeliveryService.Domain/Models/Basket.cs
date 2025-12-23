using DeliveryService.Domain.ModelsDb;

namespace DeliveryService.Domain.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public UserDb User { get; set; }

        public ICollection<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}