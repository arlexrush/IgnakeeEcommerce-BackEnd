using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.ShippingSeur;

namespace Ecommerce.UnitTests;

public class ShippingCarrierTests
{
    [Fact]
    public async Task SeurAdapterMapsCarrierResponseToDomainQuote()
    {
        var carrier = new SeurShippingCarrier(new SeurServiceStub("9.95"));

        var quote = await carrier.GetQuoteAsync(new ShippingQuoteRequest("46017", 1000, 42));

        Assert.Equal("SEUR", quote.CarrierName);
        Assert.Equal(9.95m, quote.Cost);
    }

    private sealed class SeurServiceStub : ISeurService
    {
        private readonly string _tarifa;

        public SeurServiceStub(string tarifa)
        {
            _tarifa = tarifa;
        }

        public Task<RespuestaCalculaTarifa> CalculaTarifaAsync(CalculaTarifa request)
        {
            return Task.FromResult(new RespuestaCalculaTarifa { Tarifa = _tarifa });
        }

        public Task<RespuestaPreRegistroEnvio> PreRegistro(PreRegistroEnvio request)
        {
            return Task.FromResult(new RespuestaPreRegistroEnvio());
        }

        public Task<SolicitudEtiquetaOpResponse> SolicitudEtiquetaOp(SolicitudEtiquetaOpRequest request)
        {
            return Task.FromResult(new SolicitudEtiquetaOpResponse());
        }
    }
}
