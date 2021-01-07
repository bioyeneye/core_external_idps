using aux_oauth_server.service.DataAccess.Repositories;
using aux_oauth_server.service.DataAccess.Repositories.core;
using aux_oauth_server.service.Managers;
using aux_oauth_server.service.Managers.ProviderClients;
using aux_oauth_server.service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aux_oauth_server.service
{
    /// <summary>
    /// Configure service 
    /// </summary>
    public static class ConfigureServiceCollection
    {
        /// <summary>
        /// Application service configuration
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationService(this IServiceCollection service)
        {
            service.AddTransient<IDapperCore, DapperCore>();
            service.AddTransient<IProviderRepository, ProviderRepository>();
            service.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            service.AddTransient<IProviderManager, GoogleIdentityProviderClient>();
            service.AddTransient<IProviderService, ProviderService>();
            service.AddTransient<IAuthorizationService, AuthorizationService>();
            service.AddSingleton<ProviderManager>();

            return service;
        }

        /// <summary>
        /// Get service configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="identifier"></param>
        /// <param name="exertName"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider provider, string identifier, bool exertName = false)
        {
            var services = provider.GetServices<T>();

            if (exertName)
                return services.FirstOrDefault(o => o.ToString()?.ToLower() == identifier);
            else
                return services.FirstOrDefault(o => CheckService(o.ToString(), identifier));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="compareName"></param>
        /// <returns></returns>
        private static bool CheckService(string serviceName, string compareName)
        {
            if (serviceName == null)
            {
                return false;
            }

            return serviceName.ToLower().Contains(compareName.ToLower());
        }
    }
}
