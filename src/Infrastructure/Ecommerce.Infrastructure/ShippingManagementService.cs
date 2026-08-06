using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using Ecommerce.Domain.Contracts;

namespace Ecommerce.Infrastructure;

public sealed class ShippingManagementService : IShippingManagementService
{
    private readonly IReadOnlyCollection<IShippingCarrier> _carriers;
    private readonly IUnitOfWork _unitOfWork;

    public ShippingManagementService(IEnumerable<IShippingCarrier> carriers, IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(carriers);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _carriers = carriers.ToArray();
        _unitOfWork = unitOfWork;
    }

    public async Task<PropertyInformation> SelectShippingTarifa(
        Domain.Address address,
        int pesoGramos,
        ShoppingCart shoppingCart)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(shoppingCart);

        if (_carriers.Count == 0)
        {
            throw new InvalidOperationException("No shipping carriers are configured.");
        }

        var request = new ShippingQuoteRequest(
            address.PostalCode ?? string.Empty,
            pesoGramos,
            shoppingCart.Id ?? 0);

        var quotes = await Task.WhenAll(_carriers.Select(carrier => carrier.GetQuoteAsync(request)));
        var selectedQuote = quotes.MinBy(quote => quote.Cost)
            ?? throw new InvalidOperationException("No shipping quote was returned.");

        var shippingOperator = await _unitOfWork.Repository<ShippingOperator>()
            .GetEntityAsync(
                item => item.NameShippingOperator == selectedQuote.CarrierName
                    && item.Country!.Name == address.Country,
                null,
                false);

        return new PropertyInformation
        {
            NameService = selectedQuote.CarrierName,
            OperatorName = selectedQuote.CarrierName,
            OperatorStatus = shippingOperator?.OperatorStatus ?? true,
            OrderId = shoppingCart.Id,
            TarifaShipping = selectedQuote.Cost
        };
    }
}
