using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class ItemValidator : AbstractValidator<Item>
    {
        public ItemValidator()
        {

            RuleFor(item => item.Name)
                .NotEmpty().WithMessage("Имя товара обязательно")
                .MaximumLength(100).WithMessage("Имя слишком длинное");

            RuleFor(item => item.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше 0");

            RuleFor(item => item.Description)
                .NotEmpty().WithMessage("Описание не может быть пустым");
        }
    }
}