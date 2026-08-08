using System.Security.Claims;

namespace Ecommerce.Api.AiOrchestration;

public interface IAiAssistant
{
    Task<AiAssistantResponse> AskAsync(
        AiAssistantRequest request,
        ClaimsPrincipal user,
        CancellationToken cancellationToken);
}
