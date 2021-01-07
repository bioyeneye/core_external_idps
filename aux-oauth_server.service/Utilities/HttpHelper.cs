using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aux_oauth_server.service.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Help with make network request for request that has ApplicationWwwFormUrlencoded data
        /// </summary>
        /// <typeparam name="T">Data type for result</typeparam>
        /// <typeparam name="U">Data type for error</typeparam>
        /// <param name="url">Url</param>
        /// <param name="data">post data</param>
        /// <returns></returns>
        public static async Task<GC<T, U>> MakeApplicationWwwFormUrlencodedAsync<T, U>(string url, object data)
        {
            try
            {
                var response = await url
                                .PostUrlEncodedAsync(data)
                                .ReceiveJson<T>();

                return new GC<T, U>
                {
                    Data = response,
                };
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<U>();
                return GC<T, U>.SetError(error);
            }
        }
    }

    /// <summary>
    /// Data context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public class GC<T, U>
    {
        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Error
        /// </summary>
        public U Error { get; set; }
        /// <summary>
        /// Check for data
        /// </summary>
        public bool HasData
        {
            get; private set;
        }

        /// <summary>
        /// Default
        /// </summary>
        public GC()
        {
            this.HasData = this.Data != null;
        }

        /// <summary>
        /// Set data for GC
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GC<T,U> SetData(T data)
        {
            return new GC<T, U>
            {
                Data = data
            };
        }

        /// <summary>
        /// Set error for GC
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static GC<T, U> SetError(U error)
        {
            return new GC<T, U>
            {
                Error = error,
            };
        }
    }
}
