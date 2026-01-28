using System.ComponentModel.DataAnnotations;

namespace Marketplace.Application.DTOs.Request
{
    public class OrderRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        private decimal _amount;
        public decimal Amount 
        {
            // Rounds mathematically to 2 places whenever accessed
            get => Math.Round(_amount, 2, MidpointRounding.AwayFromZero);
            set => _amount = value;
        }

        [Required]
        [RegularExpression("^(Buy|Sell|buy|sell|BUY|SELL)$", ErrorMessage = "Value must be 'Buy' or 'Sell'")]
        public string Direction { get; set; } = "";

        [Required]
        [RegularExpression("^(USD)$", ErrorMessage = "Value must be 'USD'")]
        public string BaseCurrency { get; set; } = "";

        [Required]
        [RegularExpression("^(GBP|EUR)$", ErrorMessage = "Value must be 'GBP' or 'EUR'")]
        public string QuoteCurrency { get; set; } = "";
    }
}
