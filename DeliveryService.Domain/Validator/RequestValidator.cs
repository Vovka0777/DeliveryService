using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("Описание запроса обязательно");

            RuleFor(request => request.PathImg)
                .NotEmpty().WithMessage("Путь к изображению обязателен");

            RuleFor(request => request.UserId)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор пользователя (UserId) обязателен");
        }
    }
}