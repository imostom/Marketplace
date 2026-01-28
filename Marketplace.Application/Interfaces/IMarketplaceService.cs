using LotusBank.CommunityUmmah.Application.DTOs.Request;
using Marketplace.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Interfaces
{
    public interface IMarketplaceService
    {
        Task<GenericResponseModel<Guid>> AddInventory(AddInventoryRequest request);
    }
}
