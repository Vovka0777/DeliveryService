using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            // Правило: Идентификатор клиента (IdUser) обязателен
            RuleFor(order => order.IdUser)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор клиента (IdUser) обязателен");

            // Правило: Цена должна быть больше нуля
            RuleFor(order => order.Price)
                .GreaterThan(0).WithMessage("Цена заказа должна быть больше нуля");

            // Правило: Имя (получателя) не должно быть пустым
            RuleFor(order => order.Name)
                .NotEmpty().WithMessage("Имя получателя обязательно");
        }
    }
}