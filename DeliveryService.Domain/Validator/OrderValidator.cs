using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.IdUser)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор клиента (IdUser) обязателен");

            RuleFor(order => order.Price)
                .GreaterThan(0).WithMessage("Цена заказа должна быть больше нуля");

            RuleFor(order => order.Name)
                .NotEmpty().WithMessage("Имя получателя обязательно");
        }
    }
}