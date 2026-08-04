using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Domain
{
    public class OrderItem : BaseDomainModel
    {
        public Product? Product { get; set; }
        public int ProductId { get; set; }

        [Precision(20, 2)]
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public virtual Order? Order { get; set; }
        public int OrderId { get; set; }
        public int ProductItemId { get; set; }
        public string? productName { get; set; }
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Responsable for calculating the total price of the order item based on its quantity and price.
        /// </summary>
        /// <returns>The total price of the order item.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public decimal GetLineTotal()
        {
            if (Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quantity), "Quantity must be greater than zero.");
            }

            if (Price is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Price), "Price cannot be negative.");
            }

            return (Price ?? 0m) * Quantity;
        }
    }
}
