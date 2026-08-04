using Ecommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ecommerce.Domain
{
    public class Order : BaseDomainModel
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
            ParTaxItems = new List<ParTaxItem>();
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
            OrderItems = new List<OrderItem>();
            ParTaxItems = new List<ParTaxItem>();
        }

        public string? BuyerName { get; set; }
        public string? BuyerUserName { get; set; }
        public virtual OrderAddress? OrderAddress { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<ParTaxItem>? ParTaxItems { get; set; }
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

        public void ApplyPricing(decimal? subTotal, decimal? priceTax, decimal? shippingCost)
        {
            if (subTotal is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(subTotal), "Subtotal cannot be negative.");
            }

            if (priceTax is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(priceTax), "Tax amount cannot be negative.");
            }

            if (shippingCost is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shippingCost), "Shipping cost cannot be negative.");
            }

            SubTotal = subTotal;
            PriceTax = priceTax;
            ShippingCost = shippingCost;
            Total = subTotal + priceTax + shippingCost;
        }

        public void AddItem(OrderItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (item.Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(item.Quantity), "Quantity must be greater than zero.");
            }

            if (item.Price is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(item.Price), "Price cannot be negative.");
            }

            OrderItems ??= new List<OrderItem>();
            var existingItem = OrderItems.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem is not null)
            {
                existingItem.Quantity += item.Quantity;
                existingItem.Price = item.Price;
                existingItem.productName = item.productName;
                existingItem.ImageUrl = item.ImageUrl;
                return;
            }

            item.OrderId = Id ?? item.OrderId;
            OrderItems.Add(item);
        }

        public void SetShippingDetails(string? shippingOperator, decimal? shippingCost, int? weightOrder)
        {
            if (shippingCost is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shippingCost), "Shipping cost cannot be negative.");
            }

            if (weightOrder is < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(weightOrder), "Order weight cannot be negative.");
            }

            ShippingOperator = shippingOperator;
            ShippingCost = shippingCost;
            WeightOrder = weightOrder;
        }

        public void SetPaymentDetails(string? paymentIntentId, string? clientSecret, string? stripeApiKey)
        {
            PaymentIntentId = paymentIntentId;
            ClientSecret = clientSecret;
            StripeApiKey = stripeApiKey;
        }

        public void MarkAsApproved()
        {
            orderStatus = OrderStatus.Approved;
        }

        public void MarkAsShipped()
        {
            orderStatus = OrderStatus.Shipped;
        }

        public void MarkAsError()
        {
            orderStatus = OrderStatus.Error;
        }
    }
}
