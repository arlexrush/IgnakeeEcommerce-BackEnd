using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Models.Shipping;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Stripe;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, OrderVm>
    {
        private readonly StripeSettings? _stripeSettings;
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly IShippingManagementService _shippingManagementService;
        private readonly PaymentIntentService? _paymentIntentService;
        private readonly IAuthService? _authService;
        private readonly UserManager<User>? _userManager;

        public CreatePaymentCommandHandler(IOptions<StripeSettings>? stripeSettings, IUnitOfWork? unitOfWork, IMapper? mapper, IShippingManagementService shippingManagementService, PaymentIntentService? paymentIntentService, IAuthService authService, UserManager<User> userManager)
        {
            _stripeSettings = stripeSettings!.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _shippingManagementService = shippingManagementService;
            _paymentIntentService = paymentIntentService;
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<OrderVm> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var includes = new List<Expression<Func<Order, object>>>();
            includes.Add(z => z.OrderItems!);
            includes.Add(y => y.ParTaxItems!);
            Order orderToPay = null!;
            orderToPay = await _unitOfWork!.Repository<Order>().GetEntityAsync(x => x.Id == request.OrderId, includes, false);

            if (orderToPay == null)
            {

                throw new NoFoundException("Not Found Order Requested", orderToPay!);
            }

            orderToPay.orderStatus = OrderStatus.Approved;

            await _unitOfWork!.Repository<Order>().UpdateAsync(orderToPay);

            // to do Shipping
            User? userBuyer;
            userBuyer = await _userManager!.FindByNameAsync(_authService!.GetSessionUser());

            ShippingOperator shippingOperator;
            shippingOperator = await _unitOfWork!.Repository<ShippingOperator>().GetByIdAsync(orderToPay.Id);
            var shippingServices = _mapper!.Map<PropertyInformation>(shippingOperator);

            RespuestaPreRegistroEnvio shipping;
            shipping = await _shippingManagementService.DoShipping(shippingServices, userBuyer!, orderToPay.OrderAddress!, orderToPay.WeightOrder, orderToPay);

            SolicitudEtiquetaOpResponse tagShipping;
            tagShipping = await _shippingManagementService.RequestTagShipping(shippingServices);

            var shoppingCartItems = await _unitOfWork!.Repository<ShoppingCartItem>().GetAsync(x => x.ShoppingCartMasterId == request.ShoppingCartMasterId);
            _unitOfWork.Repository<ShoppingCartItem>().DeleteRange(shoppingCartItems);
            await _unitOfWork.Complete();
            var response = _mapper!.Map<OrderVm>(orderToPay);
            return response;
        }
    }
}
