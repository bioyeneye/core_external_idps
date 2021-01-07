using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Services
{
    /// <summary>
    /// Base service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T>
    {
        private readonly ILogger<T> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        protected BaseService(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        protected void LogInformation(string message, object data)
        {
            _logger.LogInformation($"{message}: \n{JsonConvert.SerializeObject(data, Formatting.Indented)}\n");
        }

        /// <summary>
        /// Log error 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        protected void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, $"\n {message}");
        }
    }
}
