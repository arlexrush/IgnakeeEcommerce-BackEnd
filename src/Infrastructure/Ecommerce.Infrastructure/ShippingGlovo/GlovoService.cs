using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Application.Models.Shipping.Glovo;
using System.Diagnostics;
using glovo.client.csharp.Api;
using glovo.client.csharp.Client;
using glovo.client.csharp.Model;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Actions;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Domain;

namespace Ecommerce.Infrastructure.ShippingGlovo
{
    // Este servicio de Glovo, my ecommerce de alimentos recibe la orden de Glovo app y se prepara y se sube a la plataforma de glovo para que la app se encargue del cobro y del shipping.
    public class GlovoService : IGlovoService
    {
        private readonly GlovoSettings? _glovoSettings;
        private readonly Configuration? _config;
        private readonly MenuApi? _menuApi;

        public GlovoService(IOptions<GlovoSettings>? glovoSettings, Configuration? config)
        {
            _glovoSettings = glovoSettings!.Value;
            _config = config;

            config!.BasePath = _glovoSettings.UrlPath;
            config!.ApiKey.Add("Authorization", _glovoSettings.GlovoApiKey);
            _menuApi = new MenuApi(config);
        }

        public async Task<string> UploadMenu(Domain.Order order)
        {
            var storeId = _glovoSettings!.StoreId;  // string | [Unique identifier of the store](#section/Getting-started/Unique-identifier-of-the-store) 
            var contentType = "\"application/json\"";  // string | Specify that the content will be sent as JSON (default to "application/json")
            var inlineObject = new InlineObject(); // InlineObject |  (optional) 
            TransactionId result;
            string response;
            try
            {
                // Upload menu
                result = await _menuApi!.UploadMenuAsync(storeId, contentType, inlineObject);
                Debug.WriteLine(result);
                response = result._TransactionId;
                return response;
            }
            catch (ApiException e)
            {
                Debug.Print("Exception when calling MenuApi.UploadMenu: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
                throw;
            }


        }

    }
}
