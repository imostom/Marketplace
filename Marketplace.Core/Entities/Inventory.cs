using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Core.Entities
{
    public class Inventory
    {
        public Guid InventoryId { get; set; } 
        public string VendorId { get; set; } = "";
        public string BaseCurrency { get; set; } = "";
        public string QuoteCurrency { get; set; } = "";
        public decimal Rate { get; set; }
        public long Available { get; set; } 
        public DateTime DateCreated { get; set; }
    }

}