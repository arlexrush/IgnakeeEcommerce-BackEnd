using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain
{
    public class Category : BaseDomainModel
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string? Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
