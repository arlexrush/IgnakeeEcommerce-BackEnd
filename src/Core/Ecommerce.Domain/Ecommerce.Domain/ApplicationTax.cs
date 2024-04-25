using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public enum ApplicationTax
    {
        [EnumMember(Value = "Item")]
        Item,
        [EnumMember(Value = "Order")]
        Order,
    }
}
