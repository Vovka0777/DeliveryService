using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Enum
{
    public enum Role
    {
        User = 0,
        Admin = 1
    }

    public enum Status
    {
        [Display(Name = "Ожидает рассмотрения")]
        Pending = 0,

        [Display(Name = "Принят")]
        Accepted = 1,

        [Display(Name = "Отклонен")]
        Rejected = 2,

        [Display(Name = "В пути")]
        InTransit = 3,

        [Display(Name = "Доставлен")]
        Delivered = 4,

        [Display(Name = "Отменен")]
        Canceled = 5
    }

    public enum ItemType
    {
        [Display(Name = "Еда")]
        Food = 0,

        [Display(Name = "Канцелярия")]
        Drinks = 1,

        [Display(Name = "Стройматериалы")]
        Documents = 2,

        [Display(Name = "Одежда")]
        Other = 3
    }
}