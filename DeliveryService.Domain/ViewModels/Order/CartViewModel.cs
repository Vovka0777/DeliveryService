using DeliveryService.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Domain.ViewModels.Order
{
    public class CartViewModel
    {
        public Guid OrderId { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public decimal TotalPrice => Items.Sum(x => x.TotalPrice);
    }

    public class OrderItemViewModel
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }
}