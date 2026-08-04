using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Order;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Application.Models.Shipping.Mrw;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Ecommerce.Infrastructure.ShippingMrw
{
    public class MrwService : IMrwService
    {

        private readonly IHttpClientFactory? _httpClient;
        private readonly MRWSettings? _setting;
        private string _accessToken;

        public MrwService(IHttpClientFactory? httpClient, IOptions<MRWSettings>? setting)
        {
            _httpClient = httpClient;
            _setting = setting.Value;
        }



        public async Task<RespuestaPreRegistroEnvio> PreRegistro(PreRegistroEnvio request)
        {
            bool envio;
            RespuestaPreRegistroEnvio response;
            try
            {
                envio = await EnviarPaqueteAsync(request);
            }
            catch (Exception ex)
            {
                throw;
            }

            response = new RespuestaPreRegistroEnvio()
            {
                Bulto = {
                    Numbulto = 1,
                    CodEnvio = "12345678901234567890123",
                    CodManifiesto = "MDCCCCEEAAAAMMDD00000000",
                    CodEnvioIpc = "1234567890qwertyuiopasdfghjklñ12345",
                    Etiqueta = {
                        Modo = "1",
                        Etiqueta_xml = {
                            RemitenteEtiqueta = {
                                CP = "46017",
                                Direccion = "San Pio X, 36",
                                Localidad = "San Marcelin",
                                Nombre = "Arlex",
                                PersonaContacto = "Arlex Guzman",
                                Provincia = "Valencia",
                            },
                            DestinatarioEtiqueta = {
                                CP = "",
                                Direccion = "",
                                Localidad = "1",
                                Nombre = "1",
                                PersonaContacto = "1",
                                Provincia = "1",
                                Pais = "1",
                                Telefono = "",
                                ZIP = "1",
                            },
                            CodigoBarras = {
                                Fichero = Encoding.UTF8.GetBytes(""),
                                NombreF = "Etiqueta",
                                Tipo_Doc = ""
                            },
                            FechaEtiquetado = DateTime.UtcNow,
                            InstruccionesDevolucion = "",
                            Observaciones = "",
                            PesoReal = "",
                            PesoVol = "",
                            Referencia = "",
                            VA = {
                                ComplejidadGestion = 1,
                                DUA = "",
                                eAR = "",
                                EntregaConcertada = "",
                                EntregaconRecogida = "",
                                EntregaExclusiva = "",
                                EntregaSinFirmar = "",
                                FechaEntregaConcertada = "",
                                FranjaHorariaConcertada = "",
                                ImporteReembolso = "",
                                IndImprimirEtiqueta = "",
                                IntentosDeEntrega = 0, PEE = "",
                                RepartoSabado = "",
                                TarifaPlana = "",
                                TextoAdicional = "",
                                TiempoEnLista = 1 } } } },
                Alertas = new List<Alerta>(),
                CodExpedicion = "",
                EntregaParcial = "",
                FechaRespuesta = DateTime.UtcNow,
                IdiomaErrores = "",
                Resultado = 0,
                TotalBultos = 1,
                BultoError = {
                    DescError = "",
                    Error = "",
                    Numbulto = 1 }
            };

            return response;
        }

        public async Task<SolicitudEtiquetaOpResponse> SolicitudEtiquetaOp(SolicitudEtiquetaOpRequest request)
        {
            byte[] responseMRW;
            try
            {
                responseMRW = await GenerarEtiquetaEnvioAsync(request);
            }
            catch (Exception ex)
            {
                throw;
            }

            var response = new SolicitudEtiquetaOpResponse()
            {
                Bulto = {
                            Numbulto=1,
                            CodEnvio="12345678901234567890123",
                            CodManifiesto= "MDCCCCEEAAAAMMDD00000000",
                            CodEnvioIpc="1234567890qwertyuiopasdfghjklñ12345",
                            Etiqueta = {
                                            Modo="1",
                                            Etiqueta_xml = {
                                                                RemitenteEtiqueta={
                                                                                         CP="46017",
                                                                                         Direccion = "San Pio X, 36",
                                                                                         Localidad= "San Marcelin",
                                                                                         Nombre="Arlex",
                                                                                         PersonaContacto="Arlex Guzman",
                                                                                         Provincia="Valencia",
                                                                                   },
                                                                DestinatarioEtiqueta = {
                                                                                             CP="",
                                                                                             Direccion = "",
                                                                                             Localidad = "1",
                                                                                             Nombre="1",
                                                                                             PersonaContacto = "1",
                                                                                             Provincia="1",
                                                                                             Pais = "1",
                                                                                             Telefono="",
                                                                                             ZIP="1",
                                                                                        },
                                                                CodigoBarras= {
                                                                                    Fichero=responseMRW,
                                                                                    NombreF="Etiqueta",
                                                                                    Tipo_Doc=""
                                                                               },
                                                                FechaEtiquetado= DateTime.UtcNow,
                                                                InstruccionesDevolucion="",
                                                                Observaciones="",
                                                                PesoReal = "",
                                                                PesoVol = "",
                                                                Referencia = "",
                                                                VA={
                                                                        ComplejidadGestion=1,
                                                                        DUA="",
                                                                        eAR="",
                                                                        EntregaConcertada="",
                                                                        EntregaconRecogida="",
                                                                        EntregaExclusiva="",
                                                                        EntregaSinFirmar="",
                                                                        FechaEntregaConcertada="",
                                                                        FranjaHorariaConcertada="",
                                                                        ImporteReembolso="",
                                                                        IndImprimirEtiqueta="",
                                                                        IntentosDeEntrega=0, PEE="",
                                                                        RepartoSabado="",
                                                                        TarifaPlana="",
                                                                        TextoAdicional="",
                                                                        TiempoEnLista=1
                                                                    }
                                                            }
                                       }
                        },
                FechaRespuesta = DateTime.UtcNow,
                Resultado = 0,
                TotalBultos = 1,
            };

            return response;
        }

        public async Task<RespuestaCalculaTarifa> CalculaTarifaAsync(CalculaTarifa request)
        {
            decimal responseMRW;
            try
            {
                responseMRW = await SolicitarTarifaEnvioAsync(request);
            }
            catch (Exception ex)
            {
                responseMRW = 2000;
            }

            var response = new RespuestaCalculaTarifa() { Tarifa = responseMRW.ToString(), Resultado = 0, FechaRespuesta = DateTime.UtcNow };
            return response;
        }

        public async Task<decimal> SolicitarTarifaEnvioAsync(CalculaTarifa request)
        {
            _accessToken = await GetAccessTokenAsync();
            using (var client = _httpClient!.CreateClient())
            {
                client.BaseAddress = new Uri("https://api.mrw.com/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("quote", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var quote = JsonConvert.DeserializeObject<QuoteResponse>(content);
                    return (decimal)quote!.TotalPrice!;
                }
                else
                {
                    throw new Exception("Error al obtener la tarifa de envío de MRW");
                }
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var client = _httpClient!.CreateClient();

            // Realizar una solicitud a la API de MRW para obtener el token de acceso
            var requestContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("api_key", _setting!.ApiKey!)
            });

            var response = await client.PostAsync("https://api.mrw.com/auth/token", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();
                _accessToken = tokenResponse; // Guardar el token de acceso para futuras solicitudes
                return _accessToken;
            }
            else
            {
                throw new Exception("Error al obtener el token de acceso de MRW");
            }
        }

        public async Task<byte[]> GenerarEtiquetaEnvioAsync(SolicitudEtiquetaOpRequest request)
        {

            _accessToken = await GetAccessTokenAsync();
            using (var client = _httpClient!.CreateClient())
            {
                client.BaseAddress = new Uri(_setting!.ApiUrl!);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("label", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var labelBytes = await response.Content.ReadAsByteArrayAsync();
                    return labelBytes;
                }
                else
                {
                    throw new Exception("Error al generar la etiqueta de envío de MRW");
                }
            }
        }

        public async Task<bool> EnviarPaqueteAsync(PreRegistroEnvio request)
        {
            _accessToken = await GetAccessTokenAsync();
            using (var client = _httpClient!.CreateClient())
            {
                client.BaseAddress = new Uri("https://api.mrw.com/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("ship", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Error al enviar el paquete con MRW");
                }
            }
        }

    }
}
