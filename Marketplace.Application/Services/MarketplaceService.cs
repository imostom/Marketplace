using LotusBank.CommunityUmmah.Application.DTOs.Request;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Mappers;
using Marketplace.Core.Common.Models;
using Marketplace.Core.Enums;
using Marketplace.Core.Interfaces;
using Marketplace.Core.Interfaces.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Services
{
    public class MarketplaceService (ILoggerManager logger, IMarketplaceRepository marketplaceRepository) : IMarketplaceService
    {
        private readonly ILoggerManager _logger = logger;
        private readonly IMarketplaceRepository _marketplaceRepository = marketplaceRepository;
        private readonly string serviceName = "MarketplaceService";

        public async Task<GenericResponseModel<Guid>> AddInventory(AddInventoryRequest request)
        {
            var methodName = $"{serviceName}.AddInventory";
            _logger.LogInformation($"{methodName} - New request received for processing  for vendor {request.VendorId}");
            GenericResponseModel<Guid> result = new();
            try
            {
                var entityRequest = InventoryMappingExtensions.ToEntity(request);
                var response = await _marketplaceRepository.AddInventory(entityRequest);

                if (response == Guid.Empty)
                {
                    result.ResponseCode = ((int)ResponseCodes.Failed).ToString("D2");
                    result.ResponseMessage = "Failed to add inventory.";
                    return result;
                }

                result.ResponseCode = ((int)ResponseCodes.Success).ToString("D2");
                result.ResponseMessage = ResponseCodes.Success.ToString();
                result.Data = response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : {request.VendorId} | {JsonConvert.SerializeObject(ex)}");
            }

            return result;
        }
    }
}
