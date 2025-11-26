using System.Collections.Generic;

namespace DeliveryService.Domain.Filters
{
    public class ItemFilter
    {
        public decimal MaxPrice { get; set; }
        public List<int>? Categories { get; set; }
        public string Ordering { get; set; }
        
        // НОВОЕ ПОЛЕ: для поиска по названию
        public string Name { get; set; } 
    }
}