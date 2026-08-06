namespace Ecommerce.Application.Models.Shipping.Seur;

public sealed class SeurSettings
{
    public bool Enabled { get; set; }
    public string? ApiUrl { get; set; }
    public string? ApiKey { get; set; }
}
