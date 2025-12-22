using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.ViewModels.LoginAndRegistration
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Code { get; set; } 

        [Required]
        public string ConfirmCode { get; set; } 
    }
}