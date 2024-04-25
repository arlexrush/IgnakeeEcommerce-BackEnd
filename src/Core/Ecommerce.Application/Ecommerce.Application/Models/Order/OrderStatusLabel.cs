using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Models.Order
{
    public static class OrderStatusLabel
    {
        public const string? PENDING = nameof(PENDING);
        public const string? APPROVED = nameof(APPROVED);
        public const string? SHIPPED = nameof(SHIPPED);
        public const string? ERROR = nameof(ERROR);
    }
}
