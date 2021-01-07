using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace aux_oauth_server.api.Controllers
{
    /// <summary>
    /// Base controller for all api services
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]")]
    public class BaseApiController<T> : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected IServiceProvider provider;
        private readonly ILogger<T> _logger;

        /// <summary>
        /// Base api controller default constructor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        public BaseApiController(IServiceProvider provider, ILogger<T> logger)
        {
            this.provider = provider;
            _logger = logger;
        }

        /// <summary>
        /// Helps to log errors
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        protected void LogError(Exception ex, string controller, string method)
        {
            _logger.LogError(ex, $"Error from Controler: {controller}, Action Method: {method}");
        }

        /// <summary>
        /// Helps to log errors
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        protected void LogRequest(Exception ex, string controller, string method)
        {
            _logger.LogError(ex, $"Error from Controler: {controller}, Action Method: {method}");
        }
    }
}
