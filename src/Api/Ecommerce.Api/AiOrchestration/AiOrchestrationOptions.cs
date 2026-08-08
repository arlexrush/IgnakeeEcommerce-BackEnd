namespace Ecommerce.Api.AiOrchestration;

public sealed class AiOrchestrationOptions
{
    public const string SectionName = "AiOrchestration";

    public bool Enabled { get; init; }

    public string? FoundryProjectEndpoint { get; init; }

    public string ModelDeploymentName { get; init; } = "gpt-5-mini";
}
