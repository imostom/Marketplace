namespace Marketplace.Core.Entities
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public string VendorId { get; set; } = "";
        public string BaseCurrency { get; set; } = "";
        public string QuoteCurrency { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Available { get; set; }
        public DateTime DateCreated { get; set; }
    }

}