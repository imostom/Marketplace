using LotusBank.CommunityUmmah.Application.DTOs.Request;
using LotusBank.CommunityUmmah.Application.DTOs.Response;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Mappers;
using Marketplace.Core.Common.Constants;
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

        public async Task<GenericResponseModel<AddInventoryResponse>> AddInventory(AddInventoryRequest request)
        {
            var methodName = $"{serviceName}.AddInventory";
            _logger.LogInformation($"{methodName} - New request received for processing  for vendor {request.VendorId}");
            GenericResponseModel<AddInventoryResponse> result = new();
            try
            {
                var entityRequest = InventoryMappingExtensions.ToEntity(request);
                

                //get existing inventory if there is any, update instead of adding new
                var existingInventory = await _marketplaceRepository.GetInventory(request.VendorId, request.BaseCurrency, request.QuoteCurrency);
                if (existingInventory != null)
                {
                    var newRate = (existingInventory.Rate + entityRequest.Rate) / 2;
                    var newAvailable = existingInventory.Available + entityRequest.Available;
                    entityRequest.Rate = newRate;
                    entityRequest.Available = newAvailable;

                    var updateInventory = await _marketplaceRepository.UpdateInventory(entityRequest);
                    if (updateInventory != Guid.Empty)
                        return new GenericResponseModel<AddInventoryResponse>
                        {
                            ResponseCode = ((int)ResponseCodes.Success).ToString("D2"),
                            ResponseMessage = AppConstants.UpdatedAllocation,
                            Data = new AddInventoryResponse()
                            {
                                ID = updateInventory
                            }
                        };
                    else
                        return new GenericResponseModel<AddInventoryResponse>
                        {
                            ResponseCode = ((int)ResponseCodes.Failed).ToString("D2"),
                            ResponseMessage = AppConstants.AnErrorOccurred
                        };
                }

                //no existing inventory, proceed to add new
                var response = await _marketplaceRepository.AddInventory(entityRequest);

                if (response == Guid.Empty)
                {
                    result.ResponseCode = ((int)ResponseCodes.Failed).ToString("D2");
                    result.ResponseMessage = AppConstants.FailedAllocation;
                    return result;
                }

                result.ResponseCode = ((int)ResponseCodes.Success).ToString("D2");
                result.ResponseMessage = AppConstants.SuccessfulAllocation;
                result.Data = new AddInventoryResponse()
                {
                    ID = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : {request.VendorId} | {JsonConvert.SerializeObject(ex)}");
            }

            return result;
        }
    }
}
