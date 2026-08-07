using Ecommerce.Domain;

namespace Ecommerce.Application.Features.Products.Queries.Vms.Inventory
{
    /// <summary>
    /// Read-only inventory view intended for the IgnakeeAI.McpServer.Supplier integration.
    /// Contains only the fields required by the supplier adapter contract.
    /// </summary>
    public class InventoryProductVm
    {
        public string? ProductCode { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public int? Stock { get; set; }
        public string? UnitToSell { get; set; }
        public int? PurchaseLeadTime { get; set; }
        public string? PurchaseLeadTimeUnit { get; set; }
        public ProductStatus Status { get; set; }
    }
}
