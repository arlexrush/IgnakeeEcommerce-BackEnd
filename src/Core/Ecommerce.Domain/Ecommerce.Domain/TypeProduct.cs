using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public enum TypeProduct
    {
        [EnumMember(Value = "Product manufactured")]
        manufactured,
        [EnumMember(Value = "Product purchased")]
        purchased,
        [EnumMember(Value = "Product assembled")]
        assembled
    }
}
