using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LotusBank.CommunityUmmah.Application.DTOs.Request
{
    public class AddInventoryRequest
    {
        [Required]
        public string VendorId { get; set; } = "";

        [Required]
        [RegularExpression("^(USD)$", ErrorMessage = "Value must be 'USD'")]
        public string BaseCurrency { get; set; } = "";

        [Required]
        [RegularExpression("^(GBP|EUR)$", ErrorMessage = "Value must be 'GBP' or 'EUR'")]
        public string QuoteCurrency { get; set; } = "";

        [Required]
        public decimal Rate { get; set; }


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Available { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
