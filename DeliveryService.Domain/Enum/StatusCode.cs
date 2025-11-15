namespace DeliveryService.Domain.Enum
{
    public enum StatusCode
    {
        // Успешный ответ
        OK = 200,

        // Ошибки, связанные с неверным запросом пользователя
        BadRequest = 400,
        NotFound = 404,

        // Ошибки, связанные с логикой или сервером
        InternalServerError = 500,

        // Вы можете добавить другие статусы, например, Unauthorized = 401
    }
}