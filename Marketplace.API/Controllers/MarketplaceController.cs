using LotusBank.CommunityUmmah.Application.DTOs.Request;
using Marketplace.Application.Interfaces;
using Marketplace.Application.Services;
using Marketplace.Core.Common.Models;
using Marketplace.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketplaceController : BaseController
    {
        private readonly IMarketplaceService _marketplaceService;
        private readonly RemoteDetails _remoteDetails;
        private readonly ILoggerManager _logger;

        public MarketplaceController(RemoteDetails remoteDetails, IHttpContextAccessor context, IMarketplaceService marketplaceService, ILoggerManager logger, IConfiguration config) 
            : base(remoteDetails, context, logger, config)
        {
            _logger = logger;
            _remoteDetails = remoteDetails;
            _marketplaceService = marketplaceService;
        } 


        [HttpPost("addinventory")]
        public async Task<IActionResult> AddInventory([FromBody] AddInventoryRequest request)
        {
            var result = await CustomResponse(await _marketplaceService.AddInventory(request));
            return result;
        }
    }
}
