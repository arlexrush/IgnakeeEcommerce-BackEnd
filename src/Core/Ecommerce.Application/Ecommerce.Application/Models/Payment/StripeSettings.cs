namespace Ecommerce.Application.Models.Payment
{
    public class StripeSettings
    {
        public string? Publishablekey { get; set; }
        public string? SecretKey { get; set; }
        public string? WebhookSecret { get; set; }
    }
}
