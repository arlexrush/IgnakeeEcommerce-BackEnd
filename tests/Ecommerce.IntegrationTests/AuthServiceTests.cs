using System.Security.Claims;
using Ecommerce.Application.Models.Token;
using Ecommerce.Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ecommerce.IntegrationTests;

public class AuthServiceTests
{
    [Fact]
    public void GetSessionUserUsesPreferredUsernameForMicrosoftEntraTokens()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("preferred_username", "enterprise@example.com")
            }, "Entra"))
        };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new AuthService(Options.Create(new JwtSettings()), accessor);

        var result = service.GetSessionUser();

        Assert.Equal("enterprise@example.com", result);
    }

    [Fact]
    public void GetSessionUserUsesLocalNameIdentifierForLocalTokens()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "local@example.com"),
                new Claim("preferred_username", "enterprise@example.com")
            }, "Bearer"))
        };
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        var service = new AuthService(Options.Create(new JwtSettings()), accessor);

        var result = service.GetSessionUser();

        Assert.Equal("local@example.com", result);
    }
}
