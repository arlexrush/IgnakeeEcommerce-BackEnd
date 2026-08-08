using System.ComponentModel.DataAnnotations;
using Ecommerce.Application.Models.Messaging;

namespace Ecommerce.Api.BehaviorTracking;

public sealed record BehaviorTrackingRequest
{
    [Required]
    public BehaviorAction? Action { get; init; }

    [MaxLength(20)]
    public IReadOnlyCollection<int> ProductIds { get; init; } = [];

    [Range(1, int.MaxValue)]
    public int? CategoryId { get; init; }
}

public sealed record BehaviorConsentRequest(bool Granted);
