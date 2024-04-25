using Ecommerce.Api.Middlewares;
using Ecommerce.Application;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.ImageCloudinary;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);

// Add DB Configurations and connection string
builder.Services.AddDbContext<EcommerceDbContext>(options=>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"), b=>b.MigrationsAssembly(typeof(EcommerceDbContext).Assembly.FullName))); //2d parameter is for to write in console each command and query with Database.


// Add services to CQRS implementation by Mediatr lybrary
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetProductListQueryHandler).Assembly));


// Add service to Clouddinary service implementation
builder.Services.AddScoped<IManageImageService, ManageImageService>();


// Add services to the container.

builder.Services.AddControllers(opt =>
{
    // Service to Authentication
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    // Sevice to Authorize
    opt.Filters.Add(new AuthorizeFilter(policy));

}).AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);





//Identity Configuration

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>();
identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>();
identityBuilder.AddEntityFrameworkStores<EcommerceDbContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();


builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt=>
    {
        opt.TokenValidationParameters=new TokenValidationParameters {
            ValidateIssuerSigningKey= true,
            IssuerSigningKey=key,
            ValidateAudience= false,
            ValidateIssuer=false,
        };
    }
);

builder.Services.AddCors(options=>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

// upload Initial Data to BD
using (var scope=app.Services.CreateScope())
{
    var service=scope.ServiceProvider;
    var loggerFactory=service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<EcommerceDbContext>();
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager=service.GetRequiredService<RoleManager<IdentityRole>>();
        await context.Database.MigrateAsync();
        await EcommerceDbContextData.LoadDataAsync(context, userManager, roleManager, loggerFactory);
    }catch(Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Migration Error");        
    }

}


app.Run();
