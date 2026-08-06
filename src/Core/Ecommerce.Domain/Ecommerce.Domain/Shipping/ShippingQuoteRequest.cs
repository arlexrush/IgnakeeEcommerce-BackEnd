namespace Ecommerce.Domain.Contracts;

public sealed record ShippingQuoteRequest(
    string PostalCode,
    int WeightGrams,
    int ShoppingCartId);
