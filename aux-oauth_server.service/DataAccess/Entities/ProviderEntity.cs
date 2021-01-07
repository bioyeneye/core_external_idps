using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.DataAccess.Entities
{
    /// <summary>
    /// Provider entity
    /// </summary>
    public class ProviderEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuthorizationEndpoint { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string DeviceAuthorizationEndpoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TokenEndpoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UserinfoEndpoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RevocationEndpoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JwksUri { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ScopesSupported { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClaimsSupported { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GrantTypesSupported { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string Logo { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string RedirectUrl { get; internal set; }
    }
}
