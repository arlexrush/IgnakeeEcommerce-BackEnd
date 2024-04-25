using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public enum TaxStatus
    {
        [EnumMember(Value = "affected")]
        affected,
        [EnumMember(Value = "except")]
        except,
    }
}
