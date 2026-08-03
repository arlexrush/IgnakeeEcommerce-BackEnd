using AutoMapper;
using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Orders.Vms;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Models.Shipping;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Diagnostics;
using System.Linq.Expressions;
using Address = Ecommerce.Domain.Address;
using Product = Ecommerce.Domain.Product;

namespace Ecommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderVm>
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly ITaxService? _taxService;
        private readonly IMapper? _mapper;
        private readonly IAuthService? _authService;
        private readonly IShippingManagementService _shippingManagementService;
        private readonly ICorreosService? _correosService;
        private readonly UserManager<User>? _userManager;
        private readonly StripeSettings? _stripeSettings;

        public CreateOrderCommandHandler(IUnitOfWork? unitOfWork, 
                                        IMapper? mapper, 
                                        IAuthService? authService, 
                                        ICorreosService? correosService,
                                        IShippingManagementService shippingManagementService,
                                        UserManager<User>? userManager, 
                                        ITaxService? taxService, 
                                        IOptions<StripeSettings>? stripeSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
            _userManager = userManager;
            _correosService = correosService;
            _stripeSettings = stripeSettings!.Value;
            _taxService= taxService;
            _shippingManagementService = shippingManagementService;
        }

        public async Task<OrderVm> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderPending = await _unitOfWork!.Repository<Order>().GetEntityAsync(x=>x.BuyerUserName==_authService!.GetSessionUser() && x.orderStatus==OrderStatus.Pending,
                                                                                        null,
                                                                                        false
                                                                                    );

            if(orderPending is not null)
            {
                try
                {
                    await _unitOfWork.Repository<Order>().DeleteAsync(orderPending);
                }
                catch (Exception)
                {
                    throw;
                }
                
            }

            var includes = new List<Expression<Func<ShoppingCart, object>>>();
            includes.Add(x=>x.ShoppingCartItems!.OrderBy(x=>x.ProductName));
            ShoppingCart shoppingCart;
            try
            {
                shoppingCart = await _unitOfWork!.Repository<ShoppingCart>().GetEntityAsync(x => x.ShoppingCartMasterId == request.ShoppingCartId,
                                                                                            includes,
                                                                                            false);
                if (shoppingCart is null)
                {
                    throw new Exception("Not Found Shopping Cart");
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            //if(shoppingCart is not null)
            //{
            //    throw new InvalidOperationException("Exist an shopping Cart with similar id");
            //}

            var user=await _userManager!.FindByNameAsync(_authService!.GetSessionUser());
            if(user is null)
            {
                throw new InvalidOperationException("User without authentication");
            }

            var address = await _unitOfWork.Repository<Address>().GetEntityAsync(u=>u.UserName==user.UserName, null, false);
            OrderAddress orderAddress=new OrderAddress() { 
                 UserName= user.UserName,
                 City= request.AddressVm!.City??address.City,
                 Country= request.AddressVm.Country??address.Country,
                 CreatedBy= _authService.GetSessionUser(),
                 CreatedDate= DateTime.UtcNow,
                 PostalCode= request.AddressVm.PostalCode??address.PostalCode,
                 Region=  request.AddressVm.Region??address.Region,
                 UserAddress= request.AddressVm.Address??address.UserAddress
            };
            
            
            List<decimal> montoItems=new List<decimal>(); 
            List<decimal> montoTaxes=new List<decimal>();
            List<int>pesosItems=new List<int>();
            List<ParTaxItem> parTaxItems = new List<ParTaxItem>();
            List<ParTaxItem> taxSubTotals = new List<ParTaxItem>();
            Product product;
            foreach (ShoppingCartItem item in shoppingCart!.ShoppingCartItems!) {
                int productId=item.ProductId;
                
                try
                {
                    product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to load the product while creating the order.", ex);
                }

                //string countryName = product!.CountrySell!;
                string countryName = orderAddress.Country!;                                
                var includesDimensions = new List<Expression<Func<Product, object>>>();
                includesDimensions.Add(x => x.ProductDimension!);
                var productDimension = await _unitOfWork!.Repository<Product>().GetEntityAsync(x => x.Id == productId,
                                                                                                includesDimensions,
                                                                                                false);
                int pesoItem = productDimension.ProductDimension!.Weight;

                pesosItems.Add(pesoItem);
                Country country = await _unitOfWork.Repository<Country>().GetEntityAsync(x => x.Name!.Equals(countryName),null, false);
                int? countryId=country.Id;
                string currency=country.Currency!;
                List<Tax>? tasasTaxRequest=new List<Tax>();
                try
                {
                    tasasTaxRequest = await _taxService!.GetTaxesByCountryByProduct(countryId, productId);
                }catch(Exception ex)
                {
                   
                }

                Tax selectTaxRequest=new Tax();
                try
                {
                    selectTaxRequest = await _taxService!.SelectTax(tasasTaxRequest, productId, (int)countryId!);
                }
                catch(Exception ex)
                {

                }

                //if (!tasasTaxRequest.Any())
                //{
                //    decimal percentageTaxDefault = 21;
                //    Tax taxDefault = new Tax()
                //    {
                //        Percentage = percentageTaxDefault
                //    };
                //    tasasTaxRequest.Add(taxDefault);
                //    tasasTax= tasasTaxRequest;
                //}
                //else
                //{
                //    //tasasTax = tasasTaxRequest;
                //    tasasTax.Add(selectTaxRequest);
                //}

                List<Tax> tasasTax = new List<Tax>();                                
                tasasTax.Add(selectTaxRequest);
                var montoItem=Math.Round(item.Price*item.Quantity,2);
                montoItems.Add(montoItem);
                var montoWithTax=tasasTax.Select(x => ((x.Percentage/100) * montoItem)).ToList().Sum();
                var montotax = Math.Round((decimal)((montoWithTax!)), 2);
                montoTaxes.Add(montotax);
                ParTaxItem parTaxItem = new ParTaxItem() { TaxName = tasasTax.ElementAt(0).Name, TaxPercentage=tasasTax.ElementAt(0).Percentage, MontoItem=montoItem, TotalMontoItem= Math.Round((decimal)((tasasTax.ElementAt(0).Percentage! / 100) * montoItem!), 2) };
                parTaxItems.Add(parTaxItem);
            }
            var taxesGrouped=parTaxItems.GroupBy(x => x.TaxName).Select(g=>new { TaxName=g.Key, SubTotal=g.Sum(z=>z.TotalMontoItem)}).ToList();
            foreach(var tax in taxesGrouped)
            {
                taxSubTotals.Add(new ParTaxItem() { TaxName = tax.TaxName, MontoItem=null, TaxPercentage=null, TotalMontoItem=tax.SubTotal });
            }
            //var subTotal =Math.Round(shoppingCart!.ShoppingCartItems!.Sum(x => x.Price * x.Quantity), 2);
            //var taxes = Math.Round(subTotal * Convert.ToDecimal(0.18));
            var pesoGraims = pesosItems.Count == 0 ? 1000 : pesosItems.Sum();
            var subTotal=montoItems.Sum();
            var taxes=montoTaxes.Sum();


            // Selecting tarifa

            PropertyInformation tarifaShipping;

            try
            {
                tarifaShipping = await _shippingManagementService.SelectShippingTarifa(address, pesoGraims, shoppingCart);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

            // Correos Shipping Service

                                //var calculaTarifaRequest = new CalculaTarifa()
                                //{
                                //    CodEtiquetador = "",
                                //    CPDestinatario = address.PostalCode,
                                //    CPRemitente = "46017",
                                //    FechaOperacion = DateTime.UtcNow,
                                //    IdiomaErrores = "SP",
                                //    TipoPeso = "R",
                                //    Valor = pesoGraims,
                                //    CodProducto = shoppingCart.Id.ToString()
                                //};
                                //RespuestaCalculaTarifa tarifaCorreosResponse;
                                //decimal shipping;
                                //try
                                //{
                                //    tarifaCorreosResponse = await _correosService!.CalculaTarifaAsync(calculaTarifaRequest);
                
                                //}
                                //catch(Exception ex)
                                //{
                                //    tarifaCorreosResponse = new RespuestaCalculaTarifa() { Tarifa="0" };
                                //}

                                //// get Operator

                                //var shippingOperators = await _unitOfWork.Repository<ShippingOperator>().GetAsync(x=>x.Country!.Name==address.Country);


            // Cost Calculations By Correos Shipping Service
            var tarifaCorreos = tarifaShipping.TarifaShipping;
            var shippingCorreos = tarifaCorreos;
            var shipping = shippingCorreos == 0 ? 10 : shippingCorreos;
            var total = subTotal + taxes + shipping;

           

            // Creating or setting address to shipping

            var buyerName=$"{user.Name} {user.LastName}";

            var addressUserList = await _unitOfWork.Repository<Address>().GetAsync(u => u.UserName == user.UserName);
            Address newAddress=new Address();
            List<Address> newAddresses = new List<Address>();  
            foreach (var x in addressUserList)
            {

                if (orderAddress.UserName == x.UserName &&
                    orderAddress.City == x.City &&
                    orderAddress.Country == x.Country &&
                    orderAddress.PostalCode == x.PostalCode &&
                    orderAddress.Region == x.Region &&
                    orderAddress.UserAddress == x.UserAddress)
                {
                    orderAddress.UserName = x.UserName;
                    orderAddress.City = x.City;
                    orderAddress.Country = x.Country;
                    orderAddress.PostalCode = x.PostalCode;
                    orderAddress.Region = x.Region;
                    orderAddress.UserAddress = x.UserAddress;
                }
                else
                {
                    newAddress.UserName = orderAddress.UserName;
                    newAddress.City = orderAddress.City;
                    newAddress.Country = orderAddress.Country;
                    newAddress.PostalCode = orderAddress.PostalCode;
                    newAddress.Region = orderAddress.Region;
                    newAddress.UserAddress = orderAddress.UserAddress;
                    await _unitOfWork.Repository<Address>().AddAsync(newAddress);
                    break;
                }
            }

            // Creating Order to store to BBDD

            var order = new Order() { BuyerName=buyerName, 
                                        BuyerUserName=user.UserName, 
                                        OrderAddress= orderAddress, 
                                        SubTotal=subTotal, 
                                        PriceTax=taxes,
                                        ShippingCost=shipping,
                                        Total=total, 
                                        ParTaxItems=taxSubTotals,
                                        WeightOrder= pesoGraims,
                                        ShippingOperator=tarifaShipping.OperatorName};
            try
            {
                await _unitOfWork.Repository<Order>().AddAsync(order);
            }
            catch (Exception)
            {
                throw;
            }
            
            //Mapping items fron ShoppingCart to Order

            var items = new List<OrderItem>();
            foreach (var shoppingElement in shoppingCart.ShoppingCartItems)
            {
                var orderItem = new OrderItem()
                {
                    productName = shoppingElement.ProductName,
                    ProductId = shoppingElement.ProductId,
                    ImageUrl = shoppingElement.ProductPicture,
                    Price = shoppingElement.Price,
                    Quantity = shoppingElement.Quantity,
                    OrderId=(int)order.Id!,
                    //Id = shoppingElement.Id,
                };
                items.Add(orderItem);
            }

            // Storing Shipping Operator 

            var newShoppingOperator = _mapper!.Map<ShippingOperator>(tarifaShipping);
            newShoppingOperator.OperatorStatus = true;
            newShoppingOperator.OrderId = order.Id;
            newShoppingOperator.CountryName = order.OrderAddress.Country;

            try
            {
                await _unitOfWork.Repository<ShippingOperator>().AddAsync(newShoppingOperator);
            }
            catch (Exception ex)
            {
                Debug.Print("Exception when query ShippingOperator Table: " + ex.Message);
                Debug.Print(ex.StackTrace);
                throw;
            }
            

            _unitOfWork.Repository<OrderItem>().AddRange(items);
            var result=await _unitOfWork.Complete();
            if(result<=0)
            {
                throw new Exception("There is an error into Create Order Operation");
            }

            //Payment

            StripeConfiguration.ApiKey = _stripeSettings!.SecretKey;
            var service = new PaymentIntentService();
            PaymentIntent intent;
            var countryNamePayment = orderAddress.Country;
            Country countryPayment = await _unitOfWork.Repository<Country>().GetEntityAsync(x => x.Name!.Equals(countryNamePayment), null, false);
            if (String.IsNullOrEmpty(order.PaymentIntentId))
            {
                var options= new PaymentIntentCreateOptions() 
                {
                    Amount= (long)order.Total!,
                    Currency= countryPayment.Currency==null? "usd": countryPayment.Currency,
                    PaymentMethodTypes= new List<string>(){ "card" },
                };
                intent= await service.CreateAsync(options);
                order.PaymentIntentId = intent.Id;
                order.ClientSecret= intent.ClientSecret;
                order.StripeApiKey = _stripeSettings.Publishablekey;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)order.Total!,
                };
                await service.UpdateAsync(order.PaymentIntentId, options);
            }

            _unitOfWork.Repository<Order>().UpdateEntity(order);
            var orderResult=await _unitOfWork.Complete();
            if (orderResult<=0)
            {
                throw new Exception("Error while to creating strype intent of payment");
            }
            var response = _mapper!.Map<OrderVm>(order);
            return response;
        }
    }
}
