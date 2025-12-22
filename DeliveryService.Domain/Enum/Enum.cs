using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Enum
{
    // Вынесли Role из класса Enum прямо в namespace
    public enum Role
    {
        [Display(Name = "Пользователь")]
        User = 0,

        [Display(Name = "Курьер")]
        Courier = 1,

        [Display(Name = "Администратор")]
        Admin = 2
    }

    // Переименовали Status -> StatusOrder и добавили Cart/Created
    public enum StatusOrder
    {
        [Display(Name = "Корзина")]
        Cart = 0, // Важно: статус для товаров в корзине

        [Display(Name = "Создан")]
        Created = 1, // Заказ оформлен пользователем

        [Display(Name = "Ожидает рассмотрения")]
        Pending = 2,

        [Display(Name = "В обработке")]
        Processing = 3,

        [Display(Name = "Принят")]
        Accepted = 4,

        [Display(Name = "Отклонен")]
        Rejected = 5,

        [Display(Name = "В пути")]
        InTransit = 6,

        [Display(Name = "Доставлен")]
        Delivered = 7,

        [Display(Name = "Отменен")]
        Canceled = 8
    }

    public enum ItemType
    {
        [Display(Name = "Еда")]
        Food = 0,

        [Display(Name = "Напитки")]
        Drinks = 1,

        [Display(Name = "Документы")]
        Documents = 2,

        [Display(Name = "Прочее")]
        Other = 3
    }
}