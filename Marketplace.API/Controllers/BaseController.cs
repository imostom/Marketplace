using Marketplace.Core.Common.Models;
using Marketplace.Core.Enums;
using Marketplace.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Marketplace.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public RemoteDetails _remoteDetailsDTO;
        public ILoggerManager _loggerManger;
        private IConfiguration _config;
        private Stopwatch _stopWatch;

        public BaseController(RemoteDetails remoteDetailsDTO, IHttpContextAccessor httpContext, ILoggerManager loggerManger, IConfiguration config)
        {
            _config = config;
            remoteDetailsDTO.IpAddress = httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
            remoteDetailsDTO.Port = httpContext.HttpContext.Connection.RemotePort.ToString();
            remoteDetailsDTO.ApiKey = httpContext.HttpContext.Request.Headers["ApiKey"];
            remoteDetailsDTO.Path = httpContext.HttpContext.Request.Path.Value;
            remoteDetailsDTO.Channel = httpContext.HttpContext.Request.Headers["Channel"];
            _remoteDetailsDTO = remoteDetailsDTO;
            _loggerManger = loggerManger;

            _stopWatch = Stopwatch.StartNew();
        }

        protected async Task<IActionResult> CustomResponse(GenericResponseModel result)
        {
            var apiKey = _config["AppSettings:ApiKey"];

            if (apiKey != _remoteDetailsDTO.ApiKey)
            {
                result = new GenericResponseModel
                {
                    ResponseCode = ((int)ResponseCodes.Security_Violation).ToString("D2"),
                    ResponseMessage = "Unauthorized Access"
                };
            }

            _loggerManger.LogInformation($"Response to: {JsonConvert.SerializeObject(_remoteDetailsDTO)} " +
                $"Response Body : {JsonConvert.SerializeObject(result)}  TotalExecutionTime(MS):{_stopWatch.ElapsedMilliseconds}");

            var invalidCodes = new[]
            {
                ((int)ResponseCodes.Invalid_Amount).ToString("D2"),
                ((int)ResponseCodes.Invalid_Account).ToString("D2"),
                ((int)ResponseCodes.Invalid_OrderDirection).ToString("D2")
            };

            var okCodes = new[]
            {
                 ((int)ResponseCodes.Success).ToString("D2"),
                  ((int)ResponseCodes.Not_Found).ToString("D2"),
                  ((int)ResponseCodes.Not_Found).ToString("D2"),
                  ((int)ResponseCodes.Insufficient_Funds).ToString("D2"),
                  ((int)ResponseCodes.Limit_Exceeded).ToString("D2"),
                  ((int)ResponseCodes.Duplicate_Transaction).ToString("D2"),
                  ((int)ResponseCodes.No_Vendor).ToString("D2"),
            };

            if (okCodes.Contains(result.ResponseCode))
            {
                return Ok(result);
            }
            else if (result.ResponseCode == ((int)ResponseCodes.Created).ToString("D2"))
            {
                return Created("", result);
            }
            else if (result.ResponseCode == ((int)ResponseCodes.Security_Violation).ToString("D2"))
            {
                return Unauthorized(result);
            }
            else if (invalidCodes.Contains(result.ResponseCode))
            {
                return BadRequest(result);
            }

            return StatusCode(500, result);
        }
    }
}
