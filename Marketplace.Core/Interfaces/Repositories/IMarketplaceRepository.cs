
using Marketplace.Core.Entities;

namespace Marketplace.Core.Interfaces.Repositories
{
    public interface IMarketplaceRepository
    {
        Task<Guid> AddInventory(Inventory request);
        Task<List<Inventory>> GetInventory();
        Task<Inventory> GetInventory(Guid inventoryID, string vendor, string baseCurrency);
        Task<Inventory> GetInventory(string vendor, string baseCurrency, string quoteCurrency);
        Task<Guid> UpdateInventory(Inventory inventory);
    }
}