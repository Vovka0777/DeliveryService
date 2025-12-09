using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class OrderHistoryValidator : AbstractValidator<OrderHistory>
    {
        public OrderHistoryValidator()
        {
            RuleFor(history => history.IdOrder)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор заказа (IdOrder) обязателен");

            RuleFor(history => history.IdStatus)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор статуса (IdStatus) обязателен");

            RuleFor(history => history.CreatedAt)
                .NotEqual(default(DateTime)).WithMessage("Дата создания обязательна");
        }
    }
}