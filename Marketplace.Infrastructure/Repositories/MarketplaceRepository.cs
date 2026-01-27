using Azure.Core;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Core.Interfaces.Repositories;
using Marketplace.Core.Interfaces;
using Newtonsoft.Json;
using Marketplace.Core.Common.Models;
using Microsoft.Extensions.Options;
using Marketplace.Core.Entities;

namespace Marketplace.Infrastructure.Repositories
{
    public class MarketplaceRepository (ILoggerManager logger, IDapperRepository repository, IOptions<AppSettings> appSettings) : IMarketplaceRepository
    {
        private readonly ILoggerManager _logger = logger;
        private readonly IDapperRepository _repository = repository;
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly string serviceName = "MarketplaceRepository";


        public async Task<Guid> AddInventory(Inventory request)
        {
            var methodName = $"{serviceName}.AddInventory";
            _logger.LogInformation($"{methodName} - New request received for processing  for vendor {request.VendorId}");

            Guid data = Guid.Empty;
            try
            {
                var dbPara = new DynamicParameters();

                dbPara.Add("@VendorId", request.VendorId);
                dbPara.Add("@BaseCurrency", request.BaseCurrency);
                dbPara.Add("@QuoteCurrency", request.QuoteCurrency);
                dbPara.Add("@Rate", request.Rate);
                dbPara.Add("@Available", request.Available);

                data = await _repository.Insert<Guid>(_appSettings.AddInventoryProcedure, dbPara, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : {request.VendorId} | {JsonConvert.SerializeObject(ex)}");
            }

            return data;
        }

        public async Task<List<Inventory>> GetInventory()
        {
            var methodName = $"{serviceName}.GetInventory";
            _logger.LogInformation($"{methodName} - New request received for processing ");

            List<Inventory> inventories = new();
            try
            {
                var dbPara = new DynamicParameters();

                inventories = await _repository.GetAll<Inventory>(_appSettings.GetInventoryProcedure, dbPara, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - | {JsonConvert.SerializeObject(ex)}");
            }

            return inventories;
        }

        public async Task<List<Inventory>> GetInventory(Guid inventoryID)
        {
            var methodName = $"{serviceName}.GetInventory";
            _logger.LogInformation($"{methodName} - New request received for processing | {inventoryID}");

            List<Inventory> inventories = new();
            try
            {
                var dbPara = new DynamicParameters();
                dbPara.Add("@id", inventories);

                inventories = await _repository.GetAll<Inventory>(_appSettings.GetInventoryByIDProcedure, dbPara, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : InventoryID {inventoryID} | {JsonConvert.SerializeObject(ex)}");
            }

            return inventories;
        }

       
    }
}
