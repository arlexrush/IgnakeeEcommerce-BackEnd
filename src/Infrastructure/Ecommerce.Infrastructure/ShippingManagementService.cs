using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.ShoppingCarts.Vms;
using Ecommerce.Application.Models.Shipping;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.ShippingCatcherMP;
using Ecommerce.Infrastructure.ShippingCorreos;
using Ecommerce.Infrastructure.ShippingGlovo;
using Ecommerce.Infrastructure.ShippingJustEat;
using Ecommerce.Infrastructure.ShippingUberEats;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    public class ShippingManagementService: IShippingManagementService
    {
        private readonly ICorreosService? _correosService;
        private readonly IGlovoService? _glovoService;
        private readonly ICatcherMPService? _catcherMP;
        private readonly IUberEatsService? _uberEats;
        private readonly IJustEatService? _justEat;
        private readonly ISeurService? _seurService;
        private readonly IMrwService? _mrwService;
        private readonly IUpsService? _upsService;
        private readonly IDhlService? _dhlService;
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly IAuthService? _authService;
        private readonly UserManager<User>? _userManager;

        public ShippingManagementService(ICorreosService? correosService, 
                                        IGlovoService? glovoService, 
                                        ICatcherMPService? catcherMP, 
                                        IUberEatsService? uberEats, 
                                        IJustEatService? justEat, 
                                        ISeurService? seurService, 
                                        IMrwService? mrwService, 
                                        IUpsService? upsService, 
                                        IDhlService? dhlService,
                                        IUnitOfWork? unitOfWork,
                                        IMapper? mapper,
                                        IAuthService? authService,
                                        UserManager<User>? userManager)
        {
            _correosService = correosService;
            _glovoService = glovoService;
            _catcherMP = catcherMP;
            _uberEats = uberEats;
            _justEat = justEat;
            _seurService = seurService;
            _mrwService = mrwService;
            _upsService = upsService;
            _dhlService = dhlService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _userManager = userManager;
            var shippingServices = GetAllShippingServices();
        }
        public async Task<List<PropertyInformation>> GetAllShippingServices()
        {
            var userName = _authService!.GetSessionUser();
            var user=await _userManager!.FindByNameAsync(userName);
            var address = await _unitOfWork!.Repository<Domain.Address>().GetEntityAsync(u => u.UserName == user!.UserName, null, false);
            var countryShipper= await _unitOfWork!.Repository<Country>().GetEntityAsync(c => c.Name!.Equals(address.Country), null, false);

            var properties= new List<PropertyInformation>();
            var type= typeof(ShippingManagementService);
            var servicesInstances=type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach(var serv in servicesInstances)
            {   
                var prop = new PropertyInformation();
                prop.NameService = serv.Name;
                prop.Type = serv.FieldType;
                if ((serv.Name.TrimStart('_')).Contains("Service"))
                {
                    prop.OperatorName = (serv.Name.TrimStart('_')).Replace("Service", "");
                }
                
                try
                {
                    var targetShippingOperator = await _unitOfWork!.Repository<ShippingOperator>().GetEntityAsync(x => x.NameShippingOperator == prop.OperatorName, null, false);
                    
                    if (targetShippingOperator == null)
                    {
                        ShippingOperator newOperator = new ShippingOperator()
                        {
                            NameShippingOperator = prop.OperatorName,                            
                            Country=countryShipper,
                            CreatedBy = userName,
                            CreatedDate = DateTime.UtcNow                             
                        };
                        _unitOfWork.Repository<ShippingOperator>().AddEntity(newOperator);
                        await _unitOfWork.Complete();
                    }
                    prop.OperatorStatus = (await _unitOfWork!.Repository<ShippingOperator>().GetEntityAsync(x => x.NameShippingOperator == prop.OperatorName, null, false)).OperatorStatus;
                }
                catch(Exception ex)
                {
                    Debug.Print("Exception when query ShippingOperator Table: " + ex.Message);                    
                    Debug.Print(ex.StackTrace);
                    throw ex;
                }

                if (!prop.NameService.Equals("_unitOfWork") && !prop.NameService.Equals("_mapper") && !prop.NameService.Equals("_authService") && !prop.NameService.Equals("_userManager") && !prop.NameService.Equals("_glovoService") && !prop.NameService.Equals("_uberEats") && !prop.NameService.Equals("_catcherMP") && !prop.NameService.Equals("_justEat"))
                {
                    properties.Add(prop);
                }
                
            }

            return properties.Where(x=>x.OperatorStatus==true).ToList();
        }

        public async Task<PropertyInformation> SelectShippingTarifa(Domain.Address address, int pesograims, ShoppingCart shoppingCart)
        {
            decimal? tarifa;
            List<decimal?> tarifas= new List<decimal?>();
            List<PropertyInformation> servicesWithTarifa= new List<PropertyInformation>();
            var shippingServices =await GetAllShippingServices();

            var calculaTarifaRequest = new CalculaTarifa()
            {
                CodEtiquetador = "",
                CPDestinatario = address.PostalCode,
                CPRemitente = "46017",
                FechaOperacion = DateTime.UtcNow,
                IdiomaErrores = "SP",
                TipoPeso = "R",
                Valor = pesograims,
                CodProducto = shoppingCart.Id.ToString()
            };

            foreach (var serv in shippingServices)
            {
                var nameType = serv.NameService;
                var typeService = serv.Type;

                // Creamos una instancia del tipo
                object? instance = Activator.CreateInstance(typeService!);

                // Llamamos al método de prueba para demostrar el acceso a los métodos del servicio
                MethodInfo? metodo;
                try
                {
                    metodo = typeService!.GetMethod("CalculaTarifaAsync");
                }catch(Exception ex)
                {
                    Debug.Print("Exception when try access to method that manage tarifa request: " + ex.Message);
                    Debug.Print(ex.StackTrace);
                    metodo=null;
                }

                   
                // Parametros del metodo
                //object?[] parameters={ calculaTarifaRequest };
                //var tarifa = metodo!.Invoke(instance,parameters);

                if (metodo != null)
                {
                    // Parámetros del método
                    var parameters = new object[] { calculaTarifaRequest };

                    // Verificamos si el método es asíncrono y lo llamamos en consecuencia
                    if (metodo.ReturnType == typeof(Task<decimal>))
                    {
                        var task = (Task<decimal>?)metodo.Invoke(instance, parameters);
                        await task!;
                        tarifa = task.Result;
                        // Utiliza el valor de la tarifa si es necesario
                    }
                    else
                    {
                        // Si no es asíncrono, lo llamamos directamente
                        tarifa = (decimal?)metodo.Invoke(instance, parameters);
                        // Utiliza el valor de la tarifa si es necesario
                    }
                }
                else
                {
                    // Manejar la situación donde el método no existe en el servicio
                    tarifa = 2000M;
                }
                serv.TarifaShipping=tarifa;
                servicesWithTarifa.Add(serv);
            }
            var tarifaSelected= servicesWithTarifa.MaxBy(x => x.TarifaShipping);
            return tarifaSelected!;
        }

        public async Task<SolicitudEtiquetaOpResponse> RequestTagShipping(PropertyInformation service)
        {
            SolicitudEtiquetaOpRequest requestTag=new SolicitudEtiquetaOpRequest() {
                CodEtiquetador = "1234",
                CodEnvio = "12345678",
                Care = "000000",
                ModDevEtiqueta = "3"
            };

            SolicitudEtiquetaOpResponse? tagResponse;

            var nameType = service.NameService;
            var typeService = service.Type;

            // Creamos una instancia del tipo
            object? instance = Activator.CreateInstance(typeService!);

            // Llamamos al método de prueba para demostrar el acceso a los métodos del servicio
            MethodInfo? metodo;
            try
            {
                metodo = typeService!.GetMethod("SolicitudEtiquetaOp");
            }
            catch (Exception ex)
            {
                Debug.Print("Exception when try access to method that manage Tag request: " + ex.Message);
                Debug.Print(ex.StackTrace);
                metodo = null;
            }


            // Parametros del metodo
            //object?[] parameters={ calculaTarifaRequest };
            //var tarifa = metodo!.Invoke(instance,parameters);

            if (metodo != null)
            {
                // Parámetros del método
                var parameters = new object[] { requestTag };

                // Verificamos si el método es asíncrono y lo llamamos en consecuencia
                if (metodo.ReturnType == typeof(Task<SolicitudEtiquetaOpResponse>))
                {
                    var task = (Task<SolicitudEtiquetaOpResponse>?)metodo.Invoke(instance, parameters);
                    await task!;
                    tagResponse = task.Result;
                    // Utiliza el valor de la Etiqueta si es necesario
                    return tagResponse;
                }
                else
                {
                    // Si no es asíncrono, lo llamamos directamente
                    tagResponse = (SolicitudEtiquetaOpResponse?)metodo.Invoke(instance, parameters);
                    // Utiliza el valor de la tarifa si es necesario
                    return tagResponse!;
                }
            }
            else
            {
                // Manejar la situación donde el método no existe en el servicio
                throw new Exception("Dont Found method invoked");
            }
            
        }
    
        public async Task<RespuestaPreRegistroEnvio> DoShipping(PropertyInformation service, User user, OrderAddress address, int? pesograims, Order order)
        {
            var senderData = new DatosRemitente
            {
                Identificacion = new IdentificacionRemitente
                {
                    Nombre = "Timoneda",
                    Apellido1 = string.Empty,
                    Apellido2 = string.Empty,
                    Nif = "123456789012345",
                    Empresa = "zcxvqeeyeye",
                    PersonaContacto = "agshdjfjfirirjrm"
                },
                DatosDireccion = new DatosDireccionRemitente
                {
                    Bloque = string.Empty,
                    Direccion = "San Pio x, 36, San Marcelin",
                    Escalera = string.Empty,
                    Localidad = string.Empty,
                    Numero = string.Empty,
                    Piso = string.Empty,
                    Portal = string.Empty,
                    Provincia = string.Empty,
                    Puerta = string.Empty,
                    TipoDireccion = string.Empty
                },
                CP = "46017",
                ZIP = "46017",
                Pais = "SP",
                Email = "ignakee@gmail.com",
                Telefonocontacto = string.Empty,
                DatosSMS = new DatosSMSRemitente
                {
                    Idioma = string.Empty,
                    NumeroSMS = string.Empty
                }
            };

            var recipientData = new DatosDestinatario
            {
                Identificacion = new IdentificacionDestinatario
                {
                    Nombre = user.Name ?? string.Empty,
                    Apellido1 = user.LastName ?? string.Empty,
                    Apellido2 = string.Empty,
                    Nif = user.IdentityNumber ?? string.Empty,
                    Empresa = "zcxvqeeyeye",
                    PersonaContacto = user.Name ?? string.Empty
                },
                DatosDireccion = new DatosDireccionDestinatario
                {
                    Bloque = string.Empty,
                    Direccion = address.UserAddress ?? string.Empty,
                    Escalera = string.Empty,
                    Localidad = address.City ?? string.Empty,
                    Numero = string.Empty,
                    Piso = string.Empty,
                    Portal = string.Empty,
                    Provincia = address.Region ?? string.Empty,
                    Puerta = string.Empty,
                    TipoDireccion = string.Empty
                },
                DatosDireccion2 = new DatosDireccionDestinatario
                {
                    Bloque = string.Empty,
                    Direccion = address.UserAddress ?? string.Empty,
                    Escalera = string.Empty,
                    Localidad = address.City ?? string.Empty,
                    Numero = string.Empty,
                    Piso = string.Empty,
                    Portal = string.Empty,
                    Provincia = address.Region ?? string.Empty,
                    Puerta = string.Empty,
                    TipoDireccion = string.Empty
                },
                ApartadoPostaldestino = address.PostalCode ?? string.Empty,
                DestinoApartadoPostalinternacional = "N",
                CP = address.PostalCode ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Pais = address.Country ?? string.Empty,
                Telefonocontacto = user.PhoneNumber ?? string.Empty,
                ZIP = address.PostalCode ?? string.Empty,
                DatosSMS = new DatosSMSDestinatario
                {
                    Idioma = string.Empty,
                    NumeroSMS = string.Empty
                }
            };

            var envioData = new DatosEnvio
            {
                CodProducto = order.Id?.ToString() ?? string.Empty,
                TipoFranqueo = "ON",
                Pesos = new List<Peso>
                {
                    new Peso { TipoPeso = TipoPeso.Real, Valor = pesograims ?? 0 }
                }
            };

            PreRegistroEnvio requestEnvio = new PreRegistroEnvio()
            {
                FechaOperacion = DateTime.UtcNow,
                CodEtiquetador = "1234",
                NumContrato = "12345678",
                NumCliente = "12345678",
                Care = "000000",
                TotalBultos = 1,
                ModDevEtiqueta = "PDF",
                Remitente = senderData,
                CodExpedicion = string.Empty,
                CodManifiesto = string.Empty,
                Destinatario = recipientData,
                EntregaParcial = "N",
                Envio = envioData,
                IdiomaErrores = string.Empty,
            };

            RespuestaPreRegistroEnvio response;

            var nameType = service.NameService;
            var typeService = service.Type;

            // Creamos una instancia del tipo
            object? instance = Activator.CreateInstance(typeService!);

            // Llamamos al método de prueba para demostrar el acceso a los métodos del servicio
            MethodInfo? metodo;
            try
            {
                metodo = typeService!.GetMethod("PreRegistro");
            }
            catch (Exception ex)
            {
                Debug.Print("Exception when try access to method that manage Shipping: " + ex.Message);
                Debug.Print(ex.StackTrace);
                metodo = null;
            }


            // Parametros del metodo
            //object?[] parameters={ calculaTarifaRequest };
            //var tarifa = metodo!.Invoke(instance,parameters);

            if (metodo != null)
            {
                // Parámetros del método
                var parameters = new object[] { requestEnvio };

                // Verificamos si el método es asíncrono y lo llamamos en consecuencia
                if (metodo.ReturnType == typeof(Task<RespuestaPreRegistroEnvio>))
                {
                    var task = (Task<RespuestaPreRegistroEnvio>?)metodo.Invoke(instance, parameters);
                    await task!;
                    response = task.Result;
                    // Utiliza el valor de la Etiqueta si es necesario
                    return response;
                }
                else
                {
                    // Si no es asíncrono, lo llamamos directamente
                    response = (RespuestaPreRegistroEnvio?)metodo.Invoke(instance, parameters);
                    // Utiliza el valor de la tarifa si es necesario
                    return response!;
                }
            }
            else
            {
                // Manejar la situación donde el método no existe en el servicio
                throw new Exception("Dont Found method invoked");
            }

        }
    }
}
