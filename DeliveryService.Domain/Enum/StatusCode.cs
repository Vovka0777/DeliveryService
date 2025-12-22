namespace DeliveryService.Domain.Enum
{
    public enum StatusCode
    {
        // Стандартные HTTP коды
        OK = 200,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500,

        // Кастомные коды ошибок бизнес-логики
        UserNotFound = 10,
        OrderNotFound = 20,
        TaskIsHasAlready = 30,

        // Можно добавить прочие специфичные ошибки
        ItemNotFound = 40
    }
}