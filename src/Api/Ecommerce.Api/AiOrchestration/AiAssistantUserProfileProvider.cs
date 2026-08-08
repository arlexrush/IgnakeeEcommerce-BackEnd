using System.Security.Claims;

namespace Ecommerce.Api.AiOrchestration;

public sealed class AiAssistantUserProfileProvider
{
    private static readonly string[] DisplayNameClaimTypes =
    [
        "name",
        ClaimTypes.Name,
        "given_name",
        ClaimTypes.GivenName,
        "preferred_username"
    ];

    public AiAssistantUserProfile Get(ClaimsPrincipal user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var displayName = DisplayNameClaimTypes
            .Select(user.FindFirstValue)
            .Select(NormalizeDisplayName)
            .FirstOrDefault(value => value is not null);

        return new AiAssistantUserProfile(displayName);
    }

    private static string? NormalizeDisplayName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();
        return normalized.Length <= 80 && !normalized.Contains('@') ? normalized : null;
    }
}
