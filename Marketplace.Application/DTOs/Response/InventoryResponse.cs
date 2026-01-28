using Newtonsoft.Json;

namespace LotusBank.CommunityUmmah.Application.DTOs.Response
{
    public class InventoryResponse
    {
        public Guid InventoryId { get; set; }
        public string VendorId { get; set; } = "";
        public string BaseCurrency { get; set; } = "";
        public string QuoteCurrency { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Available { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
