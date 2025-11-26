using System.Collections.Generic;

namespace DeliveryService.Domain.Filters
{
    public class ItemFilter
    {
        // Максимальная цена (ползунок)
        public decimal MaxPrice { get; set; }

        // Список ID выбранных категорий (чекбоксы) 
        public List<int>? Categories { get; set; }

        // Тип сортировки (выпадающий список)
        public string Ordering { get; set; }
    }
}