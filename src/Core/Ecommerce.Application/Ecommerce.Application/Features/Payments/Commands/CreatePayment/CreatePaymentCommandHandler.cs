using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.Extensions.Options;
using Stripe;
using System.Linq.Expressions;

namespace Ecommerce.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, OrderVm>
    {
        private readonly StripeSettings? _stripeSettings;
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly PaymentIntentService? _paymentIntentService;

        public CreatePaymentCommandHandler(IOptions<StripeSettings>? stripeSettings, IUnitOfWork? unitOfWork, IMapper? mapper, PaymentIntentService? paymentIntentService)
        {
            _stripeSettings = stripeSettings!.Value;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentIntentService = paymentIntentService;
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

            if (orderToPay.PaymentStatus == PaymentStatus.Succeeded)
            {
                return _mapper!.Map<OrderVm>(orderToPay);
            }

            if (string.IsNullOrWhiteSpace(orderToPay.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = checked((long)Math.Round((orderToPay.Total ?? 0m) * 100m, MidpointRounding.ToEven)),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    Metadata = new Dictionary<string, string> { ["order_id"] = orderToPay.Id!.Value.ToString() }
                };

                var intent = await _paymentIntentService!.CreateAsync(options, cancellationToken: cancellationToken);
                orderToPay.SetPaymentDetails(intent.Id, intent.ClientSecret, _stripeSettings!.Publishablekey);
            }

            await _unitOfWork.Complete();
            var response = _mapper!.Map<OrderVm>(orderToPay);
            return response;
        }
    }
}
