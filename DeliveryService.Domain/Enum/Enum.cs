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
            [Display(Name = "Пользователь")]
            User = 0,
            [Display(Name = "Модератор")]
            Moderator = 1,
            [Display(Name = "Админ")]
            Admin = 2,
        }

        public enum Status
        {
            [Description("Не рассмотрено")]
            NotConsidered = 0,
            [Description("Одобрено")]
            Approved,
            [Description("Отказано")]
            Denied,
        }
    }
}
