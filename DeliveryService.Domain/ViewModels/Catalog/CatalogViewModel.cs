using System.Collections.Generic;
using DeliveryService.Domain.ViewModels.Item;

namespace DeliveryService.Domain.ViewModels.Catalog
{
    public class CatalogViewModel
    {
        public List<ItemViewModel> Items { get; set; }
    }
}