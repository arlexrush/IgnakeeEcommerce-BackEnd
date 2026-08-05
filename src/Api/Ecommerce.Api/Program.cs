using Ecommerce.Api.Middlewares;
using Ecommerce.Application;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Features.Products.Queries.GetProductList;
using Ecommerce.Domain;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.ImageCloudinary;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
LoadDotEnv(builder.Configuration, builder.Environment.ContentRootPath);

builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);

// Add DB Configurations and connection string
builder.Services.AddDbContext<EcommerceDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection")
        ?? builder.Configuration.GetConnectionString("ConnectionString")
        ?? throw new InvalidOperationException("A PostgreSQL connection string is required.");

    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(EcommerceDbContext).Assembly.FullName));
});

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

}).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);




//Identity Configuration

IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>();
identityBuilder = new IdentityBuilder(identityBuilder.UserType, identityBuilder.Services);

identityBuilder.AddRoles<IdentityRole>().AddDefaultTokenProviders();
identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>();
identityBuilder.AddEntityFrameworkStores<EcommerceDbContext>();
identityBuilder.AddSignInManager<SignInManager<User>>();


builder.Services.TryAddSingleton(TimeProvider.System);
var jwtKey = builder.Configuration["JwtSettings:Key"]
    ?? throw new InvalidOperationException("JwtSettings:Key is required.");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
const string localJwtScheme = JwtBearerDefaults.AuthenticationScheme;
const string entraScheme = "Entra";
const string smartScheme = "Smart";
const string googleScheme = "Google";
var entraEnabled = builder.Configuration.GetValue<bool>("AzureAd:Enabled");
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
var googleEnabled = !string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret);
var googleCallbackPath = builder.Configuration["Authentication:Google:CallbackPath"] ?? "/signin-google";
var publicBaseUrl = builder.Configuration["Authentication:PublicBaseUrl"];
var publicBaseUri = ValidateGoogleConfiguration(
    googleEnabled,
    googleCallbackPath,
    publicBaseUrl,
    builder.Environment.IsDevelopment());

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;

    var configuredProxies = builder.Configuration["ForwardedHeaders:KnownProxies"]?
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (configuredProxies is not null)
    {
        foreach (var proxy in configuredProxies)
        {
            if (IPAddress.TryParse(proxy, out var address))
            {
                options.KnownProxies.Add(address);
            }
        }
    }
});

var authenticationBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = smartScheme;
    options.DefaultChallengeScheme = smartScheme;
})
    .AddPolicyScheme(smartScheme, null, options =>
    {
        options.ForwardDefaultSelector = context => entraEnabled && IsMicrosoftEntraToken(context.Request.Headers.Authorization)
            ? entraScheme
            : localJwtScheme;
    })
    .AddJwtBearer(localJwtScheme, opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateAudience = false,
            ValidateIssuer = false,
        };
    });

if (googleEnabled)
{
    authenticationBuilder
        .AddCookie(IdentityConstants.ExternalScheme)
        .AddGoogle(googleScheme, options =>
        {
            options.ClientId = googleClientId!;
            options.ClientSecret = googleClientSecret!;
            options.SignInScheme = IdentityConstants.ExternalScheme;
            options.SaveTokens = false;
            options.CallbackPath = googleCallbackPath;
            options.Scope.Add("email");
            options.ClaimActions.MapJsonKey("email_verified", "email_verified");
            options.Events.OnRedirectToAuthorizationEndpoint = context =>
            {
                var redirectUri = BuildPublicCallbackUri(publicBaseUri!, googleCallbackPath);
                var authorizationUri = ReplaceQueryParameter(context.RedirectUri, "redirect_uri", redirectUri);
                context.Response.Redirect(authorizationUri);
                return Task.CompletedTask;
            };
        });
}

if (entraEnabled)
{
    authenticationBuilder.AddMicrosoftIdentityWebApi(
        builder.Configuration.GetSection("AzureAd"),
        entraScheme);
}

builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseForwardedHeaders();

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
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<EcommerceDbContext>();
        var userManager = service.GetRequiredService<UserManager<User>>();
        var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await context.Database.MigrateAsync();
        await EcommerceDbContextData.LoadDataAsync(context, userManager, roleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Migration Error");
    }

}


app.Run();

static void LoadDotEnv(IConfigurationManager configuration, string contentRootPath)
{
    var directory = new DirectoryInfo(contentRootPath);
    while (directory is not null)
    {
        var envFile = Path.Combine(directory.FullName, ".env");
        if (File.Exists(envFile))
        {
            foreach (var line in File.ReadLines(envFile))
            {
                var valueLine = line.Trim();
                if (valueLine.Length == 0 || valueLine.StartsWith('#'))
                {
                    continue;
                }

                var separatorIndex = valueLine.IndexOf('=');
                if (separatorIndex <= 0)
                {
                    continue;
                }

                var key = valueLine[..separatorIndex].Trim();
                var value = valueLine[(separatorIndex + 1)..].Trim();
                if (value.Length >= 2 && value[0] == '"' && value[^1] == '"')
                {
                    value = value[1..^1];
                }

                if (Environment.GetEnvironmentVariable(key) is null)
                {
                    Environment.SetEnvironmentVariable(key, value);
                }
            }

            configuration.AddEnvironmentVariables();
            return;
        }

        directory = directory.Parent;
    }
}

static bool IsMicrosoftEntraToken(string? authorizationHeader)
{
    if (string.IsNullOrWhiteSpace(authorizationHeader) ||
        !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
    {
        return false;
    }

    var token = authorizationHeader["Bearer ".Length..].Trim();
    if (!new JwtSecurityTokenHandler().CanReadToken(token))
    {
        return false;
    }

    var issuer = new JwtSecurityTokenHandler().ReadJwtToken(token).Issuer;
    return Uri.TryCreate(issuer, UriKind.Absolute, out var issuerUri) &&
        (issuerUri.Host.Equals("login.microsoftonline.com", StringComparison.OrdinalIgnoreCase) ||
         issuerUri.Host.Equals("sts.windows.net", StringComparison.OrdinalIgnoreCase));
}

static Uri? ValidateGoogleConfiguration(
    bool googleEnabled,
    string callbackPath,
    string? publicBaseUrl,
    bool isDevelopment)
{
    if (!googleEnabled)
    {
        return null;
    }

    if (!callbackPath.StartsWith('/') || callbackPath.Contains('?') || callbackPath.Contains('#'))
    {
        throw new InvalidOperationException("Authentication:Google:CallbackPath must be an absolute path without query or fragment.");
    }

    if (!Uri.TryCreate(publicBaseUrl, UriKind.Absolute, out var publicBaseUri) ||
        publicBaseUri is null ||
        !string.IsNullOrEmpty(publicBaseUri.Query) ||
        !string.IsNullOrEmpty(publicBaseUri.Fragment) ||
        !string.IsNullOrEmpty(publicBaseUri.UserInfo))
    {
        throw new InvalidOperationException("Authentication:PublicBaseUrl must be an absolute base URL without query, fragment or credentials.");
    }

    if (!isDevelopment && !publicBaseUri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException("Authentication:PublicBaseUrl must use HTTPS outside Development.");
    }

    return publicBaseUri;
}

static string BuildPublicCallbackUri(Uri publicBaseUri, string callbackPath)
{
    return $"{publicBaseUri.AbsoluteUri.TrimEnd('/')}{callbackPath}";
}

static string ReplaceQueryParameter(string authorizationUri, string parameterName, string parameterValue)
{
    var uri = new Uri(authorizationUri);
    var query = QueryHelpers.ParseQuery(uri.Query)
        .ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
    query[parameterName] = parameterValue;

    return QueryHelpers.AddQueryString(uri.GetLeftPart(UriPartial.Path), query);
}
