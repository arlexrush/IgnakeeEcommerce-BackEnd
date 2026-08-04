using AutoMapper;
using Ecommerce.Application.Features.Addresses.Vms;
using Ecommerce.Application.Features.Countries.Queries.Vm;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Commands.Vms;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Features.Reviews.Command.CreateReview;
using Ecommerce.Application.Features.Shippings.Vms;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Features.Taxes.Vms;
using Ecommerce.Application.Models.Order;
using Ecommerce.Application.Models.Shipping;
using Ecommerce.Domain;

namespace Ecommerce.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductVm>()
            .ForMember(p => p.CategoryNombre, x => x.MapFrom(a => a.Category!.Name))
            .ForMember(p => p.NumeroReviews, x => x.MapFrom(a => a.Reviews == null ? 0 : a.Reviews.Count));

            CreateMap<Image, ImageVm>();
            CreateMap<Review, ReviewVm>();
            //CreateMap<Product, ProductVm>();
            CreateMap<ShippingAddressVm, Address>();
            CreateMap<Country, CountryVm>();
            CreateMap<Category, CategoryVm>();
            CreateMap<CreateProductImageCommand, Image>();
            //CreateMap<CreateProductCommand, Product>();
            CreateMap<CreateProductCommand, Product>()
                .ForMember(d => d.ProductName, x => x.MapFrom(o => o.ProductName))
                .ForMember(d => d.Price, x => x.MapFrom(o => o.ProductPrice))
                .ForMember(d => d.Description, x => x.MapFrom(o => o.ProductDescription))
                .ForMember(d => d.SellerName, x => x.MapFrom(o => o.ProductSeller))
                .ForMember(d => d.CountrySell, x => x.MapFrom(p => p.CountrySell))
                .ForMember(d => d.Stock, x => x.MapFrom(o => o.Stock))
                .ForMember(d => d.CategoryId, x => x.MapFrom(o => o.CategoryId));
            CreateMap<UpdateProductCommand, Product>()
                .ForMember(d => d.Id, x => x.MapFrom(o => o.Id))
                .ForMember(d => d.ProductName, x => x.MapFrom(o => o.ProductName))
                .ForMember(d => d.Price, x => x.MapFrom(o => o.ProductPrice))
                .ForMember(d => d.Description, x => x.MapFrom(o => o.ProductDescription))
                .ForMember(d => d.SellerName, x => x.MapFrom(o => o.ProductSeller))
                .ForMember(d => d.CountrySell, x => x.MapFrom(p => p.CountrySell))
                .ForMember(d => d.Stock, x => x.MapFrom(o => o.Stock))
                .ForMember(d => d.CategoryId, x => x.MapFrom(o => o.CategoryId));
            CreateMap<CreateReviewCommand, Review>();
            CreateMap<ShoppingCart, ShoppingCartVm>()
                .ForMember(d => d.ShoppingCartId, x => x.MapFrom(o => o.ShoppingCartMasterId))
                .ForMember(d => d.Items, x => x.MapFrom(o => o.ShoppingCartItems));
            CreateMap<ShoppingCartItem, ShoppingCartItemVm>();
            CreateMap<ShoppingCartItemVm, ShoppingCartItem>();
            CreateMap<Address, ShippingAddressVm>()
                .ForMember(d => d.Address, sr => sr.MapFrom(s => s.UserAddress));
            CreateMap<Order, OrderVm>()
                .ForMember(x => x.ShippingAddress, y => y.MapFrom(z => z.OrderAddress))
                .ForMember(x => x.Taxes, y => y.MapFrom(z => z.PriceTax))
                .ForMember(x => x.Shipping, y => y.MapFrom(z => z.ShippingCost))
                .ForMember(x => x.Status, y => y.MapFrom(z => z.orderStatus));
            CreateMap<OrderVm, Order>()
                .ForMember(x => x.OrderAddress, y => y.MapFrom(z => z.ShippingAddress))
                .ForMember(x => x.PriceTax, y => y.MapFrom(z => z.Taxes))
                .ForMember(x => x.ShippingCost, y => y.MapFrom(z => z.Shipping))
                .ForMember(x => x.orderStatus, y => y.MapFrom(z => z.Status));
            CreateMap<OrderItem, OrderItemVm>();
            CreateMap<OrderItemVm, OrderItemVm>();
            CreateMap<OrderAddress, ShippingAddressVm>()
                .ForMember(x => x.Address, y => y.MapFrom(z => z.UserAddress));
            CreateMap<ShippingAddressVm, OrderAddress>()
                .ForMember(x => x.UserAddress, y => y.MapFrom(z => z.Address));
            CreateMap<Address, ShippingAddressVm>()
                .ForMember(x => x.Address, y => y.MapFrom(z => z.UserAddress));
            CreateMap<ShippingAddressVm, Address>()
                .ForMember(x => x.UserAddress, y => y.MapFrom(z => z.Address));
            CreateMap<Country, CountryVm>();
            CreateMap<CountryVm, Country>();
            CreateMap<Tax, TaxVm>();
            CreateMap<TaxVm, Tax>();
            CreateMap<TaxByProduct, TaxByProductVm>();
            CreateMap<TaxByProductVm, TaxByProduct>();
            CreateMap<ProductDimension, ProductDimensionVm>();
            CreateMap<ProductDimensionVm, ProductDimension>();
            CreateMap<ParTaxItem, ParTaxItemVm>();
            CreateMap<ParTaxItemVm, ParTaxItem>();
            CreateMap<Shipping, ShippingVm>();
            CreateMap<ShippingVm, Shipping>();
            CreateMap<ShippingOperator, ShippingOperatorVm>();
            CreateMap<PropertyInformation, ShippingOperator>()
                .ForMember(x => x.NameShippingOperator, y => y.MapFrom(z => z.OperatorName));
            CreateMap<ShippingOperator, PropertyInformation>()
                .ForMember(x => x.OperatorName, y => y.MapFrom(z => z.NameShippingOperator));
        }
    }
}
