using LotusBank.CommunityUmmah.Application.DTOs.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusBank.CommunityUmmah.Application.DTOs.Response
{
    public class InventoryResponse
    {
        public Guid InventoryId { get; set; }
        public string VendorId { get; set; } = "";
        public string BaseCurrency { get; set; } = "";
        public string QuoteCurrency { get; set; } = "";
        public decimal Rate { get; set; }
        public long Available { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
