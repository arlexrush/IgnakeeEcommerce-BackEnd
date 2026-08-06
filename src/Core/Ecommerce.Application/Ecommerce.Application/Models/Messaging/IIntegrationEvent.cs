namespace Ecommerce.Application.Models.Messaging;

public interface IIntegrationEvent
{
    string EventType { get; }
    int ContractVersion { get; }
}
