using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace aux_oauth_server.service.Responses
{
    /// <summary>
    /// Configured oauth Identity Provider(idp) in the system 
    /// </summary>
    public class ProviderResponse
    {
        /// <summary>
        /// Id of the idp in the system
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }
        /// <summary>
        /// /ClientId
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Redirect Url
        /// </summary>
        public string[] RedirectUrl { get; set; }
        /// <summary>
        /// Scope for the access token like openid, profile, email, address, phone, offline_access etc
        /// Note: for refresh token you must add offline_access
        /// </summary>
        public string[] Scope { get; set; }
        /// <summary>
        /// Grant type to use
        /// </summary>
       // public GrantType[] GrantTypeAllowed { get; set; }
        public string[] GrantTypeAllowed { get; set; }
        /// <summary>
        /// Url for provider's grant type
        /// </summary>
        public ClientGrantUrl[] ClientGrantUrls { get; set; }
        /// <summary>
        /// Client secret
        /// </summary>
        [JsonIgnore]
        public string Secret { get; internal set; }
    }

    /// <summary>
    /// Grant type allowed by the idp
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// Authorization code
        /// </summary>
        [Description("Authorization Code")]
        authorization_code,

        /// <summary>
        /// Implicit Flow
        /// </summary>
        [Description("Implicit Flow")]
        implicit_flow,

        /// <summary>
        /// Client credential flow
        /// </summary>
        [Description("CLIENT CREDENTIAL FLOW")]
        client_credential_flow,
        
        /// <summary>
        /// Client credential flow
        /// </summary>
        [Description("refresh_token")]
        refresh_token,
    }

    /// <summary>
    /// The usl for the provider grant type
    /// </summary>
    public class ClientGrantUrl
    {
        /// <summary>
        /// Grant type for the provier url
        /// </summary>
        //public GrantType GrantType { get; set; }
        public string GrantType { get; set; }
        /// <summary>
        /// Url for the provider grant type
        /// </summary>
        public string Url { get; set; }
    }
}
