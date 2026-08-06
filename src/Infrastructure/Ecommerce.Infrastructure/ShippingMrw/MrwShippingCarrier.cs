using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Domain.Contracts;

namespace Ecommerce.Infrastructure.ShippingMrw;

public sealed class MrwShippingCarrier : IShippingCarrier
{
    private readonly IMrwService _service;

    public MrwShippingCarrier(IMrwService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        _service = service;
    }

    public string Name => "MRW";

    public async Task<ShippingQuote> GetQuoteAsync(ShippingQuoteRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _service.CalculaTarifaAsync(new CalculaTarifa
        {
            CPDestinatario = request.PostalCode,
            CPRemitente = "46017",
            FechaOperacion = DateTime.UtcNow,
            IdiomaErrores = "SP",
            TipoPeso = "R",
            Valor = request.WeightGrams,
            CodProducto = request.ShoppingCartId.ToString()
        });

        if (!decimal.TryParse(response.Tarifa, out var cost))
        {
            throw new InvalidOperationException("MRW returned an invalid shipping quote.");
        }

        return new ShippingQuote(Name, cost);
    }
}
