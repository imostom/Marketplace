
using Marketplace.Core.Entities;

namespace Marketplace.Core.Interfaces.Repositories
{
    public interface IMarketplaceRepository
    {
        Task<Guid> AddInventory(Inventory request);
        Task<List<Inventory>> GetInventory();
        Task<List<Inventory>> GetInventory(Guid inventoryID);
    }
}