using LotusBank.CommunityUmmah.Application.DTOs.Request;
using LotusBank.CommunityUmmah.Application.DTOs.Response;
using Marketplace.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Mappers
{
    public static class InventoryMappingExtensions
    {
        public static Inventory ToEntity(this AddInventoryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return new Inventory
            {
                VendorId = request.VendorId,
                BaseCurrency = request.BaseCurrency,
                QuoteCurrency = request.QuoteCurrency,
                Rate = request.Rate,
                Available = request.Available
            };
        }


        public static InventoryResponse ToDto(this Inventory entity)
        {
            if (entity == null) return null;

            return new InventoryResponse
            {
                InventoryId = entity.InventoryId,
                VendorId = entity.VendorId,
                BaseCurrency = entity.BaseCurrency,
                QuoteCurrency = entity.QuoteCurrency,
                Rate = entity.Rate,
                Available = entity.Available
            };
        }


        public static List<InventoryResponse> ToDto(this List<Inventory> entity)
        {
            if (entity == null) return null;

            List<InventoryResponse> listData = new();
            foreach (var e in entity)
                listData.Add(ToDto(e));

            return listData;
        }
    }
}
