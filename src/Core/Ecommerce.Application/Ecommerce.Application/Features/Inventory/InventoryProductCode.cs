using Ecommerce.Domain;

namespace Ecommerce.Application.Features.Inventory
{
    public static class InventoryProductCode
    {
        public const string SyntheticPrefix = "product-";

        public static string? Resolve(Product product)
        {
            if (!string.IsNullOrWhiteSpace(product.ProductCode))
            {
                return product.ProductCode;
            }

            return product.Id.HasValue ? $"{SyntheticPrefix}{product.Id.Value}" : null;
        }

        public static bool TryParseSynthetic(string productCode, out int productId)
        {
            productId = 0;

            if (string.IsNullOrWhiteSpace(productCode) ||
                !productCode.StartsWith(SyntheticPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return int.TryParse(productCode[SyntheticPrefix.Length..], out productId) && productId > 0;
        }
    }
}
