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
    public class Address:BaseDomainModel
    {
        [Column(TypeName = "NVARCHAR(4000)")]
        public string? UserAddress { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? City { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? Region { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? PostalCode { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? UserName { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? Country { get; set; }
        
    
    }
}
