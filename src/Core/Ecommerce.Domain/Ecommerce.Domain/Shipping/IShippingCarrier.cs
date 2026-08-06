namespace Ecommerce.Domain.Contracts;

public interface IShippingCarrier
{
    string Name { get; }

    Task<ShippingQuote> GetQuoteAsync(ShippingQuoteRequest request, CancellationToken cancellationToken = default);
}
