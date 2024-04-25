using Ecommerce.Application.Contracts.Identity;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.Email;
using Ecommerce.Application.Models.ImageMangement;
using Ecommerce.Application.Models.Payment;
using Ecommerce.Application.Models.Shipping.Correos;
using Ecommerce.Application.Models.Shipping.Dhl;
using Ecommerce.Application.Models.Shipping.Glovo;
using Ecommerce.Application.Models.Shipping.Mrw;
using Ecommerce.Application.Models.Token;
using Ecommerce.Application.Persistence;
using Ecommerce.Infrastructure.MessageImplementation;
using Ecommerce.Infrastructure.ProductTaxes;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Services.Auth;
using Ecommerce.Infrastructure.ShippingCatcherMP;
using Ecommerce.Infrastructure.ShippingCorreos;
using Ecommerce.Infrastructure.ShippingDhl;
using Ecommerce.Infrastructure.ShippingGlovo;
using Ecommerce.Infrastructure.ShippingJustEat;
using Ecommerce.Infrastructure.ShippingMrw;
using Ecommerce.Infrastructure.ShippingSeur;
using Ecommerce.Infrastructure.ShippingUberEats;
using Ecommerce.Infrastructure.ShippingUps;
using glovo.client.csharp.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace Ecommerce.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITaxService, TaxService>();
            services.AddTransient<ICorreosService, CorreosService>();
            services.AddTransient<IDhlService, DhlService>();
            services.AddTransient<IMrwService, MrwService>();
            services.AddTransient<ISeurService, SeurService>();
            services.AddTransient<IShippingManagementService, ShippingManagementService>();
            services.AddTransient<IGlovoService, GlovoService>();
            services.AddTransient<IJustEatService, JustEatService>();
            services.AddTransient<ICatcherMPService, CatcherMPService>();
            services.AddTransient<IUberEatsService, UberEatsService>();
            services.AddTransient<IUpsService, UpsService>();
            services.AddTransient<Configuration>();
            services.AddTransient<PaymentIntentService>();
            services.AddHttpContextAccessor();
            

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));
            services.Configure<CorreosSettings>(configuration.GetSection("CorreosSettings"));
            services.Configure<GlovoSettings>(configuration.GetSection("GlovoSettings"));
            services.Configure<MRWSettings>(configuration.GetSection("MRWSettings"));
            services.Configure<DHLSettings>(configuration.GetSection("DHLSettings"));

            return services;
        }
    }
}
