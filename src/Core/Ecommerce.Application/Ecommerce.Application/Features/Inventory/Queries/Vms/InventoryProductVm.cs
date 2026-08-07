namespace Ecommerce.Application.Features.Inventory.Queries.Vms
{
    public class InventoryProductVm
    {
        public string? ProductCode { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public bool IsAvailableForSale { get; set; }
        public int? Stock { get; set; }
        public string? UnitToSell { get; set; }
        public int? PurchaseLeadTime { get; set; }
        public string? PurchaseLeadTimeUnit { get; set; }
        public string? Status { get; set; }
    }
}
