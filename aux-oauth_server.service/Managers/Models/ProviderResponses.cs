using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Managers.Models
{
    /// <summary>
    /// Standard idp 
    /// </summary>
    public class StandardSuccessProviderResponses
    {
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// The audience of the token.
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// An opaque refresh token. This is returned if the offline_access scope is granted
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// The expiration time of the access token in seconds.
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// An ID token. This is returned if the openid scope is granted.
        /// </summary>
        public string id_token { get; set; }
        /// <summary>
        /// The scopes contained in the access token.
        /// </summary>
        public string scope { get; set; }
    }

    /// <summary>
    /// Standard error response
    /// </summary>
    public class StandardErrorProviderResponses
    {
        /// <summary>
        /// Error key
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// Error desciption
        /// </summary>
        public string error_description { get; set; }
    }
}
