using Marketplace.Application.DTOs.Request;
using Marketplace.Core.Entities;

namespace Marketplace.Application.Helpers
{
    public class OrderHelper
    {
        public static List<Inventory> SortVendors(List<Inventory> vendorInventory, OrderRequest request)
        {
            List<Inventory> sortedVendors = new();

            if (request.Direction.ToLower() == "buy")
            {
                //for buy orders, get vendors with lowest rates first
                sortedVendors = vendorInventory
                    .Where(x => x.BaseCurrency.ToLower() == request.BaseCurrency.ToLower()
                            && x.QuoteCurrency.ToLower() == request.QuoteCurrency.ToLower()
                            && x.Available > 0)
                    .OrderBy(x => x.Rate)
                    .ToList();
            }
            else if (request.Direction.ToLower() == "sell")
            {
                //for sell orders, get vendors with highest rates first
                sortedVendors = vendorInventory
                    .Where(x => x.BaseCurrency.ToLower() == request.BaseCurrency.ToLower()
                            && x.QuoteCurrency.ToLower() == request.QuoteCurrency.ToLower()
                            && x.Available > 0)
                    .OrderByDescending(x => x.Rate)
                    .ToList();
            }
            return sortedVendors;
        }

    }
}
