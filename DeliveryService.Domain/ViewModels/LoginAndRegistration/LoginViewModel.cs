using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.LoginAndRegistration
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин или почту")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}