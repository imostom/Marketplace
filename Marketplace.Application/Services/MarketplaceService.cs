using LotusBank.CommunityUmmah.Application.DTOs.Request;
using LotusBank.CommunityUmmah.Application.DTOs.Response;
using Marketplace.Application.DTOs.Request;
using Marketplace.Application.DTOs.Response;
using Marketplace.Application.Helpers;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Mappers;
using Marketplace.Core.Common.Constants;
using Marketplace.Core.Common.Models;
using Marketplace.Core.Entities;
using Marketplace.Core.Enums;
using Marketplace.Core.Interfaces;
using Marketplace.Core.Interfaces.Repositories;
using Newtonsoft.Json;

namespace Marketplace.Application.Services
{
    public class MarketplaceService(ILoggerManager logger, IMarketplaceRepository marketplaceRepository) : IMarketplaceService
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

        public async Task<GenericResponseModel<OrderResponse>> Order(OrderRequest request)
        {
            var methodName = $"{serviceName}.AddInventory";
            _logger.LogInformation($"{methodName} - New request received for processing  for {request.Amount}");
            GenericResponseModel<OrderResponse> result = new();
            try
            {

                var vendorInventory = await _marketplaceRepository.GetInventory();
                if (vendorInventory == null || vendorInventory.Count == 0)
                {
                    result.ResponseCode = ((int)ResponseCodes.No_Vendor).ToString("D2");
                    result.ResponseMessage = AppConstants.OrderProcessingNoVendor;
                    return result;
                }

                //there are vendors, filter those that can fulfil the order
                List<Inventory> sortedVendors = OrderHelper.SortVendors(vendorInventory, request);


                //get total available amount
                var totalAvailableAmount = sortedVendors.Sum(x => x.Available);
                if (totalAvailableAmount < request.Amount)
                {
                    _logger.LogInformation($"{methodName} - Insufficient available amount to process order");
                    result.ResponseCode = ((int)ResponseCodes.Insufficient_Funds).ToString("D2");
                    result.ResponseMessage = AppConstants.OrderProccessingInsufficient;
                    return result;
                }

                _logger.LogInformation($"{methodName} - {sortedVendors.Count} ready to fulfil order");

                if (request.Direction.ToLower() == "buy")
                {
                    result = await ProcessBuy(sortedVendors, request);
                    _logger.LogInformation($"{methodName} - done processing buy order");
                }
                else
                {
                    _logger.LogInformation($"{methodName} - done processing buy order");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : {request.Amount} | {JsonConvert.SerializeObject(ex)}");
            }

            return result;
        }



        private async Task<GenericResponseModel<OrderResponse>> ProcessBuy(List<Inventory> sortedVendors, OrderRequest request)
        {
            var methodName = $"{serviceName}.ProcessBuy";
            decimal totalRate = 0m;
            decimal totalCost = 0m;
            decimal vendorThatFulfilled = 0m;
            decimal allocatableAmount = 0m;
            var remainingOrderAmount = request.Amount;
            List<Breakdown> breakdown = new();
            var orderId = Guid.NewGuid().ToString();
            GenericResponseModel<OrderResponse> result = new();

            foreach (var s in sortedVendors)
            {
                var tag = $"OrderId: {orderId} | Vendor: {s.VendorId} | AmountAvailable: {s.Available} | Rate: {s.Rate}";
                _logger.LogInformation($"{methodName} - Processing {tag}");

                if (remainingOrderAmount <= 0)
                    break;

                if (s.Available <= 0)
                    continue;

                allocatableAmount = Math.Min(s.Available, remainingOrderAmount);
                s.Available -= allocatableAmount;
                remainingOrderAmount -= allocatableAmount;
                totalRate = totalRate + s.Rate;
                totalCost = totalCost + (allocatableAmount * s.Rate);

                _logger.LogInformation($"{methodName} - Processing {tag} \nRemaining Amount: {remainingOrderAmount}");

                breakdown.Add(new Breakdown()
                {
                    VendorId = s.VendorId,
                    AmountFilled = allocatableAmount,
                    Rate = s.Rate,
                    Cost = allocatableAmount * s.Rate
                });

                vendorThatFulfilled = vendorThatFulfilled + 1;

                //update inventory
                _logger.LogInformation($"{methodName} - Updating inventory record for {s.VendorId} \n{tag} \nRemaining Amount: {remainingOrderAmount}");
                await _marketplaceRepository.UpdateInventory(s);
                _logger.LogInformation($"{methodName} - Done updating inventory record for {s.VendorId} \n{tag} \nRemaining Amount: {remainingOrderAmount}");

                //log order execution - omitted for brevity
            }

            result.ResponseCode = ((int)ResponseCodes.Success).ToString("D2");
            result.ResponseMessage = AppConstants.OrderProcessedSuccess;
            result.Data = new OrderResponse()
            {
                OrderId = orderId,
                Status = AppConstants.OrderStatus,
                RequestDetails = new Requestdetails()
                {
                    Amount = request.Amount,
                    BaseCurrency = request.BaseCurrency,
                    QuoteCurrency = request.QuoteCurrency,
                    Direction = request.Direction
                },
                ExecutionDetails = new Executiondetails()
                {
                    TotalCostQuoteCurrency = totalCost,
                    WeightedAverageRate = totalRate / vendorThatFulfilled,
                    Breakdown = breakdown
                }
            };

            return result;
        }
    }
}
