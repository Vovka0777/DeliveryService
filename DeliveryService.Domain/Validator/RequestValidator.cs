using FluentValidation;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Validator
{
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            // Правило: Описание не должно быть пустым
            RuleFor(request => request.Description)
                .NotEmpty().WithMessage("Описание запроса обязательно");

            // Правило: Путь к изображению не должен быть пустым
            RuleFor(request => request.PathImg)
                .NotEmpty().WithMessage("Путь к изображению обязателен");

            // Дополнительное правило: Проверка, что IdUser не является Guid.Empty
            RuleFor(request => request.UserId)
                .NotEqual(Guid.Empty).WithMessage("Идентификатор пользователя (UserId) обязателен");
        }
    }
}