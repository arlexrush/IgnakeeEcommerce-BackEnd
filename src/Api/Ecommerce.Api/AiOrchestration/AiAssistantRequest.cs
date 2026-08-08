using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.AiOrchestration;

public sealed record AiAssistantRequest(
    [property: Required, StringLength(4_000, MinimumLength = 1)] string Message,
    AiPageContext? PageContext = null);
