using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string? Email { get; set; }

        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [Display(Name = "Адрес доставки")]
        public string? Address { get; set; }

        [Display(Name = "Аватар")]
        public string? AvatarPath { get; set; }

        [Display(Name = "Загрузить фото")]
        public IFormFile? AvatarFile { get; set; }
    }
}