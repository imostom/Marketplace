using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusBank.CommunityUmmah.Application.DTOs.Request
{
    public class AddInventoryRequest
    {
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
