using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Commons
{
    public abstract class BaseDomainModel
    {
        public int? Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [MaxLength(100)]
        public string? LastModifiedBy { get; set; }

    }
}
