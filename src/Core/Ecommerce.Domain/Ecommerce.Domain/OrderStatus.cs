using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Order Pending")]
        Pending,
        [EnumMember(Value = "Order Completed and Approved")]
        Approved,
        [EnumMember(Value = "Order Shipped")]
        Shipped,
        [EnumMember(Value = "Order with Errors")]
        Error

    }
}
