using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
