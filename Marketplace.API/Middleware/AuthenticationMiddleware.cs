using Marketplace.Core.Common.Constants;
using Marketplace.Core.Common.Models;
using Marketplace.Core.Enums;
using Marketplace.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Marketplace.API.Middleware
{
    public class AuthenticationMiddleware
    {
        //public List<Client> Clients { get; set; }
    }

    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ApiKeyMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var config = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = config["AppSettings:ApiKey"];
            GenericResponseModel response = new()
            {
                ResponseCode = ((int)ResponseCodes.Security_Violation).ToString("D2"),
                ResponseMessage = ResponseCodes.Security_Violation.ToString()
            };

            var path = context.Request.Path.Value.TrimStart('/');

            if (!context.Request.Path.Value.Contains("swagger") && !context.Request.Path.Value.Contains("openapi") && !context.Request.Path.Value.Contains("css"))
            {
                var clientApiKey = "";

                //mobile & branch
                if (!context.Request.Headers.TryGetValue(AppConstants.ApiKey, out var extractedApiKey))
                {
                    _logger.LogInformation($"AuthenticationMiddleware - Token isn't provided ");

                    response.ResponseCode = ((int)ResponseCodes.Security_Violation).ToString("D2");
                    response.ResponseMessage = "Token isn't provided";

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }

                if (string.IsNullOrEmpty(extractedApiKey) || extractedApiKey != apiKey)
                {
                    _logger.LogInformation($"AuthenticationMiddleware - Invalid token provided");

                    response.ResponseCode = ((int)ResponseCodes.Security_Violation).ToString("D2");
                    response.ResponseMessage = "Invalid token provided";

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }


                await _next(context);
            }
            else
            {
                var clientApiKey = "";

                //mobile & branch
                if (!context.Request.Headers.TryGetValue(AppConstants.ApiKey, out var extractedApiKey))
                {
                    _logger.LogInformation($"AuthenticationMiddleware - Token isn't provided ");

                    response.ResponseCode = ((int)ResponseCodes.Security_Violation).ToString("D2");
                    response.ResponseMessage = "Token isn't provided";

                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsJsonAsync(response);
                    return;
                }


                await _next(context);
            }


        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}

