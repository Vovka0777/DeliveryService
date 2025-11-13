using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Domain.Enum
{
    public class Enum
    {
        public enum Role
        {
            [Display(Name = "Пользователь")] // User
            User = 0,

            [Display(Name = "Курьер")] // Courier
            Courier = 1,

            [Display(Name = "Администратор")] // Administrator
            Admin = 2
        }

        // Перечисление для статуса заказа (order_history) и запроса (request.status)
        public enum Status
        {
            [Display(Name = "Ожидает рассмотрения")] // Pending Review
            Pending = 0,

            [Display(Name = "Принят")] // Accepted
            Accepted = 1,

            [Display(Name = "Отклонен")] // Rejected
            Rejected = 2,

            // Дополнительные статусы для заказа, если требуются:
            [Display(Name = "В пути")] // In Transit
            InTransit = 3,

            [Display(Name = "Доставлен")] // Delivered
            Delivered = 4,

            [Display(Name = "Отменен")] // Canceled
            Canceled = 5
        }

        // Перечисление для типа товара (item.type, если это нужно для категоризации) - Опционально
        public enum ItemType
        {
            [Display(Name = "Еда")] // Food
            Food = 0,

            [Display(Name = "Напитки")] // Drinks
            Drinks = 1,

            [Display(Name = "Документы")] // Documents
            Documents = 2,

            [Display(Name = "Прочее")] // Other
            Other = 3
        }
    }
}
