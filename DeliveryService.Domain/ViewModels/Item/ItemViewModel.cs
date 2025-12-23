using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DeliveryService.Domain.Enum;

namespace DeliveryService.Domain.ViewModels.Item
{
    public class ItemViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Цена должна быть больше 0")]
        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }

        [Display(Name = "Категория")]
        public ItemType Category { get; set; } 

        public string PathImg { get; set; }

        public IFormFile? Avatar { get; set; }

    }
}