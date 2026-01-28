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
                var query = "INSERT INTO Inventory (inventoryId, VendorId, BaseCurrency, QuoteCurrency, Rate, Available, DateCreated) " +
                    "OUTPUT Inserted.InventoryId " +
                    "VALUES (@InventoryId, @VendorId, @BaseCurrency, @QuoteCurrency, @Rate, @Available, @DateCreated)";

                var dbPara = new DynamicParameters();

                dbPara.Add("@InventoryId", Guid.NewGuid());
                dbPara.Add("@VendorId", request.VendorId);
                dbPara.Add("@BaseCurrency", request.BaseCurrency);
                dbPara.Add("@QuoteCurrency", request.QuoteCurrency);
                dbPara.Add("@Rate", request.Rate);
                dbPara.Add("@Available", request.Available);
                dbPara.Add("@DateCreated", DateTime.Now);

                data = await _repository.Insert<Guid>(query, dbPara, commandType: CommandType.Text);
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
                var query = "SELECT * FROM Inventory with(nolock)";
                var dbPara = new DynamicParameters();

                inventories = await _repository.GetAll<Inventory>(query, dbPara, commandType: CommandType.Text);
                //inventories = await _repository.GetAll<Inventory>(_appSettings.GetInventoryProcedure, dbPara, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - | {JsonConvert.SerializeObject(ex)}");
            }

            return inventories;
        }

        public async Task<Inventory> GetInventory(Guid inventoryID, string vendor, string baseCurrency)
        {
            var methodName = $"{serviceName}.GetInventory";
            _logger.LogInformation($"{methodName} - New request received for processing | {inventoryID}");

            Inventory inventory = new();
            try
            {
                var query = "SELECT * FROM Inventory with(nolock) WHERE VendorId = @VendorId AND BaseCurrency = @BaseCurrency AND InventoryId = @InventoryId";

                var dbPara = new DynamicParameters();
                dbPara.Add("@InventoryId", inventoryID);
                dbPara.Add("@BaseCurrency", baseCurrency);
                dbPara.Add("@VendorId", vendor);

                inventory = await _repository.Get<Inventory>(query, dbPara, commandType: CommandType.Text);
                //inventory = await _repository.Get<Inventory>(_appSettings.GetInventoryByIDProcedure, dbPara, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : InventoryID {inventoryID} | {JsonConvert.SerializeObject(ex)}");
            }

            return inventory;
        }

        public async Task<Inventory> GetInventory(string vendor, string baseCurrency, string quoteCurrency)
        {
            var methodName = $"{serviceName}.GetInventory";
            _logger.LogInformation($"{methodName} - New request received for processing | {vendor}");

            Inventory inventory = new();
            try
            {
                var query = "SELECT * FROM Inventory with(nolock) WHERE VendorId = @VendorId AND BaseCurrency = @BaseCurrency AND QuoteCurrency = @QuoteCurrency";

                var dbPara = new DynamicParameters();
                dbPara.Add("@QuoteCurrency", quoteCurrency);
                dbPara.Add("@BaseCurrency", baseCurrency);
                dbPara.Add("@VendorId", vendor);

                inventory = await _repository.Get<Inventory>(query, dbPara, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : InventoryID {vendor} | {JsonConvert.SerializeObject(ex)}");
            }

            return inventory;
        }

        public async Task<Guid> UpdateInventory(Inventory inventory)
        {
            var methodName = $"{serviceName}.UpdateInventory";
            _logger.LogInformation($"{methodName} - New request received for processing | {inventory.VendorId}");

            Guid inventoryId = new();
            try
            {
                var query = "UPDATE Inventory " +
                    "SET Available = @Available, Rate = @Rate " +
                    "OUTPUT Inserted.InventoryId " +
                    "WHERE LOWER(VendorId) = LOWER(@VendorId) " +
                    "AND LOWER(BaseCurrency) = LOWER(@BaseCurrency) " +
                    "AND LOWER(QuoteCurrency) = LOWER(@QuoteCurrency)";

                var dbPara = new DynamicParameters();
                dbPara.Add("@Available", inventory.Available);
                dbPara.Add("@Rate", inventory.Rate);
                dbPara.Add("@QuoteCurrency", inventory.QuoteCurrency);
                dbPara.Add("@BaseCurrency", inventory.BaseCurrency);
                dbPara.Add("@VendorId", inventory.VendorId);

                inventoryId = await _repository.Insert<Guid>(query, dbPara, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} - ERROR - : InventoryID {inventory.VendorId} | {JsonConvert.SerializeObject(ex)}");
            }

            return inventoryId;
        }
    }
}
