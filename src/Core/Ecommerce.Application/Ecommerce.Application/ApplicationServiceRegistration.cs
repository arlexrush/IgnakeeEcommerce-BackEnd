using AutoMapper;
using Ecommerce.Application.Behaviors;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Mapping;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            var mapperConfig=new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper=mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpClient();


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandleExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<PaymentIntentService>();

            return services;
        }
    }
}
