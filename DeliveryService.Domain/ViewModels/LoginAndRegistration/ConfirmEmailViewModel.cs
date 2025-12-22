using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.LoginAndRegistration
{
    public class ConfirmEmailViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите код")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Подтвердите код")]
        public int ConfirmCode { get; set; }

        public string Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}