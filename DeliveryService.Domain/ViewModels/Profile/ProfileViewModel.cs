using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Укажите логин")]
        [MinLength(3, ErrorMessage = "Логин должен быть длиннее 3 символов")]
        [MaxLength(20, ErrorMessage = "Логин должен быть меньше 20 символов")]
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

        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [MinLength(6, ErrorMessage = "Пароль должен быть больше 6 символов")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmNewPassword { get; set; }
    }
}