namespace Marketplace.Application.DTOs.Response
{
    public class OrderResponse
    {
        public string OrderId { get; set; } = "";
        public string Status { get; set; } = "";
        public Requestdetails RequestDetails { get; set; }
        public Executiondetails ExecutionDetails { get; set; }
    }

    public class Requestdetails
    {
        public string BaseCurrency { get; set; } = "";
        public string QuoteCurrency { get; set; } = "";
        public decimal Amount { get; set; }
        public string Direction { get; set; } = "";
    }

    public class Executiondetails
    {
        public decimal WeightedAverageRate { get; set; }
        public decimal TotalCostQuoteCurrency { get; set; }
        public List<Breakdown> Breakdown { get; set; }
    }

    public class Breakdown
    {
        public string VendorId { get; set; } = "";
        public decimal AmountFilled { get; set; }
        public decimal Rate { get; set; }
        public decimal Cost { get; set; }
    }

}
