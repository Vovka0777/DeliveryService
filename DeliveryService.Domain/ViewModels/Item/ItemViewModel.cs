using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.Item
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public string PathImg { get; set; }
    }
}