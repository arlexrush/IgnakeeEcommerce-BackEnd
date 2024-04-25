using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Domain
{
    public class Order:BaseDomainModel
    {
        public Order()
        {
        }

        public Order(string? buyerName, 
                    string? buyerUserName, 
                    OrderAddress? orderAddress, 
                    decimal? subTotal, 
                    decimal? total, 
                    decimal? priceTax, 
                    decimal? shippingCost)
        {
            BuyerName = buyerName;
            BuyerUserName = buyerUserName;
            OrderAddress = orderAddress;
            SubTotal = subTotal;
            Total = total;
            PriceTax = priceTax;
            ShippingCost = shippingCost;
        }

        public string? BuyerName { get; set; }
        public string? BuyerUserName { get; set; }
        public virtual OrderAddress? OrderAddress { get; set; } 
        public virtual IReadOnlyList<OrderItem>? OrderItems { get; set; }
        public virtual IReadOnlyList<ParTaxItem>? ParTaxItems { get; set; }        
        public virtual Shipping? Shipping { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? SubTotal { get; set; }
        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? Total { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? PriceTax { get; set; }
        [Column(TypeName = "INT")]
        public int? WeightOrder { get; set; }
        public string? ShippingOperator { get; set; }
        [Column(TypeName = "DECIMAL(20,2)")]
        public decimal? ShippingCost { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? StripeApiKey { get; set; }


    }
}
