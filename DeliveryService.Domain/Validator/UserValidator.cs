using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() 
        {
            RuleFor(user => user.Password).NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email");
        }
    }
}
