using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Products.Queries.Vms
{
    public class ReviewVm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int ProductId { get; set; }
        
    }
}
