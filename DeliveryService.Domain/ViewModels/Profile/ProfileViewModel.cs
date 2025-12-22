using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DeliveryService.Domain.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Имя / Логин")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public string AvatarPath { get; set; }

        [Display(Name = "Загрузить аватар")]
        public IFormFile Avatar { get; set; }

        // Поля для смены пароля (если нужно)
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}