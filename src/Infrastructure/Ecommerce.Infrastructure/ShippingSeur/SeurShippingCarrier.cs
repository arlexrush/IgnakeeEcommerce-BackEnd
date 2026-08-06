using System.Globalization;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Domain.Contracts;

namespace Ecommerce.Infrastructure.ShippingSeur;

public sealed class SeurShippingCarrier : IShippingCarrier
{
    private readonly ISeurService _service;

    public SeurShippingCarrier(ISeurService service)
    {
        ArgumentNullException.ThrowIfNull(service);
        _service = service;
    }

    public string Name => "SEUR";

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
            CodProducto = request.ShoppingCartId.ToString(CultureInfo.InvariantCulture)
        });

        if (!decimal.TryParse(response.Tarifa, NumberStyles.Any, CultureInfo.InvariantCulture, out var cost))
        {
            throw new InvalidOperationException("SEUR returned an invalid shipping quote.");
        }

        return new ShippingQuote(Name, cost);
    }
}
