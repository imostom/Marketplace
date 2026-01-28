using LotusBank.CommunityUmmah.Application.DTOs.Request;
using LotusBank.CommunityUmmah.Application.DTOs.Response;
using Marketplace.Application.DTOs.Request;
using Marketplace.Application.DTOs.Response;
using Marketplace.Core.Common.Models;

namespace Marketplace.Application.Interfaces
{
    public interface IMarketplaceService
    {
        Task<GenericResponseModel<AddInventoryResponse>> AddInventory(AddInventoryRequest request);
        Task<GenericResponseModel<OrderResponse>> Order(OrderRequest request);
    }
}
