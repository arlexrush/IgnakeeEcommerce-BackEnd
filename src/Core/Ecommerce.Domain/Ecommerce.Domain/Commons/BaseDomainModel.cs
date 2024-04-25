using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Commons
{
    public abstract class BaseDomainModel
    {
        [Column(TypeName = "INT")]
        public int? Id { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? CreatedDate { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? CreatedBy { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? LastModifiedDate { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LastModifiedBy { get; set;}

    }
}
