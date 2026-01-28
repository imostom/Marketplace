using Marketplace.Application.Interfaces;
using Marketplace.Application.Services;
using Marketplace.Core.Common.Models;
using Marketplace.Core.Interfaces;
using Marketplace.Core.Interfaces.Repositories;
using Marketplace.Infrastructure.Data.Context;
using Marketplace.Infrastructure.Logging;
using Marketplace.Infrastructure.Repositories;

namespace Marketplace.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<RemoteDetails>();
            services.AddSingleton<DapperContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IDapperRepository, DapperRepository>();
            services.AddScoped<IMarketplaceService, MarketplaceService>();
            services.AddScoped<IMarketplaceRepository, MarketplaceRepository>();


            return services;
        }
    }
}
