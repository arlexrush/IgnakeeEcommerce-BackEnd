using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Shipping.Correos;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.ShippingCorreos
{
    public class CorreosService : ICorreosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly CorreosSettings _setting;

        public CorreosService(IHttpClientFactory httpClient, IOptions<CorreosSettings> setting)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient)); ;
            _setting = setting.Value ?? throw new ArgumentNullException(nameof(setting));
        }

        public async Task<RespuestaCalculaTarifa> CalculaTarifaAsync(CalculaTarifa request)
        {
            var client=_httpClient.CreateClient();

            // Configura la llamada HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://preregistroenvios.correos.es/preregistroenvios/CalculaTarifa");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _setting.CertificadoDigital);
            
            // Serializa el objeto request
            XmlSerializer xlmSerializer = new XmlSerializer(typeof(CalculaTarifa));
            using (var writer = new StringWriter())
            {
                xlmSerializer.Serialize(writer, request);
                var xml = writer.ToString();

                // Establece el contenido de la solicitud
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "application/xml");
               
            }
            // Realiza la llamada HTTP
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            // Procesa la respuesta HTTP
            if (responseMessage.IsSuccessStatusCode)
            {
                // Devuelve la respuesta

                var xmlResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                RespuestaCalculaTarifa response;
                XmlSerializer rXlmSerializer = new XmlSerializer(typeof(RespuestaCalculaTarifa));
                using (var reader = XmlReader.Create(new StringReader(xmlResponse)))
                {
                    response = (RespuestaCalculaTarifa)rXlmSerializer.Deserialize(reader)!;

                }

                return response;

            }
            else
            {
                // Manejo de errores HTTP
                HandleHttpException(responseMessage.StatusCode, responseMessage.ReasonPhrase!);
                
            }
            return null!;
        }
        private void HandleHttpException(HttpStatusCode statusCode, string reasonPhrase)
        {
            switch (statusCode)
            {
                case HttpStatusCode.BadRequest: throw new Exception("Error en la Solicitud: " + reasonPhrase);
                case HttpStatusCode.Unauthorized: throw new Exception("Error de Autenticación: " + reasonPhrase);
                case HttpStatusCode.NotFound: throw new Exception("Recurso no encontrado: " + reasonPhrase);
                case HttpStatusCode.Forbidden: throw new Exception("Acceso Denegado: " + reasonPhrase);
                default: throw new Exception("Error Desconocido: " + reasonPhrase);
            }
        }

        public async Task<RespuestaPreRegistroEnvio> PreRegistro(PreRegistroEnvio request)
        {
            var client=_httpClient.CreateClient();

            // Configura la llamada HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://preregistroenvios.correos.es/preregistroenvios/PreRegistro");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _setting.CertificadoDigital);

            // Serializa el objeto request
            XmlSerializer xlmSerializer = new XmlSerializer(typeof(PreRegistroEnvio));
            using (var writer = new StringWriter())
            {
                xlmSerializer.Serialize(writer, request);
                var xml = writer.ToString();

                // Establece el contenido de la solicitud
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "application/xml");

            }

            // Realiza la llamada HTTP
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            // Procesa la respuesta HTTP
            if (responseMessage.IsSuccessStatusCode)
            {
                // Devuelve la respuesta

                var xmlResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                RespuestaPreRegistroEnvio response;
                XmlSerializer rXlmSerializer = new XmlSerializer(typeof(RespuestaPreRegistroEnvio));
                using (var reader = XmlReader.Create(new StringReader(xmlResponse)))
                {
                    response = (RespuestaPreRegistroEnvio)rXlmSerializer.Deserialize(reader)!;

                }

                return response;

            }
            else
            {
                // Manejo de errores HTTP
                HandleHttpException(responseMessage.StatusCode, responseMessage.ReasonPhrase!);

            }
            return null!;
        }


        public async Task<SolicitudEtiquetaOpResponse> SolicitudEtiquetaOp(SolicitudEtiquetaOpRequest request)
        {
            var client = _httpClient.CreateClient();

            // Configura la llamada HTTP
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://preregistroenvios.correos.es/preregistroenvios/PreRegistro");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _setting.CertificadoDigital);

            // Serializa el objeto request
            XmlSerializer xlmSerializer = new XmlSerializer(typeof(SolicitudEtiquetaOpRequest));
            using (var writer = new StringWriter())
            {
                xlmSerializer.Serialize(writer, request);
                var xml = writer.ToString();

                // Establece el contenido de la solicitud
                requestMessage.Content = new StringContent(xml, Encoding.UTF8, "application/xml");

            }

            // Realiza la llamada HTTP
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);

            // Procesa la respuesta HTTP
            if (responseMessage.IsSuccessStatusCode)
            {
                // Devuelve la respuesta

                var xmlResponse = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                SolicitudEtiquetaOpResponse response;
                XmlSerializer rXlmSerializer = new XmlSerializer(typeof(SolicitudEtiquetaOpResponse));
                using (var reader = XmlReader.Create(new StringReader(xmlResponse)))
                {
                    response = (SolicitudEtiquetaOpResponse)rXlmSerializer.Deserialize(reader)!;

                }

                return response;

            }
            else
            {
                // Manejo de errores HTTP
                HandleHttpException(responseMessage.StatusCode, responseMessage.ReasonPhrase!);

            }
            return null!;
            
        }
    }
}
