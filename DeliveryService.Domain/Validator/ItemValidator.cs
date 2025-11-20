using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {
            // Правило: Идентификатор заказа (IdOrder) обязателен
            RuleFor(item => item.IdOrder)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор заказа (IdOrder) обязателен");

            // Правило: Имя товара (Name) не должно быть пустым
            RuleFor(item => item.Name)
                .NotEmpty().WithMessage("Имя товара обязательно");

            // Если PathImg является обязательным, добавьте:
            /*
            RuleFor(item => item.PathImg)
                .NotEmpty().WithMessage("Путь к изображению товара обязателен");
            */
        }
    }
}