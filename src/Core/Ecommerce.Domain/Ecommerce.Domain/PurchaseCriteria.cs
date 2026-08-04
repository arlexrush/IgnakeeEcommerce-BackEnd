using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public enum PurchaseCriteria
    {
        [EnumMember(Value = "Purchase by Stock")]
        Stock,
        [EnumMember(Value = "Purchace by Order")]
        Order
    }
}
