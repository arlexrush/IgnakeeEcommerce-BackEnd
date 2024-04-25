using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Shippings.Vms;
using Ecommerce.Application.Models.Order;
using Ecommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Features.Orders.Vms
{
    public class OrderVm
    {
        public int Id { get; set; }
        public ShippingAddressVm? ShippingAddress { get; set; }
        public List<OrderItemVm>? OrderItems { get; set; }
        public IReadOnlyList<ParTaxItem>? ParTaxItems { get; set; }        
        public ShippingVm? ShippingServices { get; set; }
        public string? ShippingOperator { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Total { get; set; }
        public decimal? Shipping { get; set; }

        public OrderStatus Status { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public string? StripeApiKey { get; set; }

        public string? BuyerUserName { get; set; }
        public string? BuyerName { get; set; }

        public int? Quantity {  get{ return OrderItems!.Sum(x => x.Quantity); }
                                set{ } 
                             }
        public string? StatusLabel {get{
                                            switch(Status)
                                            {
                                                case OrderStatus.Approved:
                                                {
                                                    return OrderStatusLabel.APPROVED;
                                                }
                                                case OrderStatus.Pending:
                                                {
                                                    return OrderStatusLabel.PENDING;
                                                }
                                                case OrderStatus.Shipped:
                                                {
                                                    return OrderStatusLabel.SHIPPED;
                                                }
                                                case OrderStatus.Error:
                                                {
                                                    return OrderStatusLabel.ERROR;
                                                }
                                                default: return OrderStatusLabel.ERROR;
                                            }; 
                                       } 
                                    set{ } 
                                   }
        
    }
}
