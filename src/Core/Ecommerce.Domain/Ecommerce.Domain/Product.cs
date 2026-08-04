using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Domain
{
    public class Product : BaseDomainModel
    {
        public Product()
        {
            Reviews = new List<Review>();
            ProductImages = new List<Image>();
            TaxByProducts = new List<TaxByProduct>();
        }

        [MaxLength(100)]
        public string? ProductCode { get; set; }

        [MaxLength(100)]
        public string? ProductName { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }

        public TypeProduct typeProduct { get; set; }

        [MaxLength(100)]
        public string? UnitToSell { get; set; }

        [MaxLength(100)]
        public string? UnitToBuy { get; set; }

        [MaxLength(100)]
        public string? UnitToStore { get; set; }

        [MaxLength(100)]
        public string? UnitToProduction { get; set; }

        [MaxLength(100)]
        public string? Currency { get; set; }

        [Precision(20, 2)]
        public decimal? Price { get; set; }

        public int? Rating { get; set; }

        public int? RatingTotal { get; set; }

        [MaxLength(100)]
        public string? ProviderName { get; set; }

        [MaxLength(100)]
        public string? SellerName { get; set; }

        [MaxLength(100)]
        public string? CountrySell { get; set; }

        public PurchaseCriteria PurchaseCriteria { get; set; }

        public int? Stock { get; set; }

        public int? PurchaseLot { get; set; }

        public int? PurchaseLeadTime { get; set; }

        [MaxLength(100)]
        public string? PurchaseLeadTimeUnit { get; set; }

        [MaxLength(100)]
        public string? ReplenishmentPoint { get; set; }

        public int? SafetyStock { get; set; }

        public int? ProductDimensionId { get; set; }
        public virtual ProductDimension? ProductDimension { get; set; }

        public ProductStatus Status { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        [MaxLength(100)]
        public string? BarCode { get; set; }

        [MaxLength(100)]
        public string? QrCode { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Image>? ProductImages { get; set; }
        public virtual ICollection<TaxByProduct>? TaxByProducts { get; set; }

        public void SetBasicInformation(string? productCode, string? productName, string? description)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new ArgumentException("Product code is required.", nameof(productCode));
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name is required.", nameof(productName));
            }

            ProductCode = productCode;
            ProductName = productName;
            Description = description;
        }

        public void SetPrice(decimal? price)
        {
            if (price is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            }

            Price = price;
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }

            Stock = (Stock ?? 0) + quantity;
        }

        public void ReserveStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }

            var availableStock = Stock ?? 0;
            if (availableStock < quantity)
            {
                throw new InvalidOperationException("Not enough stock available for this operation.");
            }

            Stock = availableStock - quantity;
        }

        public void SetStatus(ProductStatus status)
        {
            Status = status;
        }
    }
}
