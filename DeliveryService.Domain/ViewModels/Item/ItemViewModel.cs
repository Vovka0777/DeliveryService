using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Domain.ViewModels.Item
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; } // Можно выводить строковое название

        public string PathImg { get; set; } // Путь к изображению

        public IFormFile? Avatar { get; set; } // Для загрузки (если понадобится позже)
    }
}