namespace Ecommerce.Domain.Contracts;

public sealed record ShippingQuote(
    string CarrierName,
    decimal Cost);
