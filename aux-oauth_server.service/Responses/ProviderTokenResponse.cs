using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Responses
{
    /// <summary>
    /// 
    /// </summary>
    public class ProviderTokenResponse
    {
        /// <summary>
        /// /
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// An ID token. This is returned if the openid scope is granted.
        /// </summary>
        public string IdToken { get; set; }
        /// <summary>
        /// The audience of the token.
        /// </summary>
        public string TokenType { get; set; }
        /// <summary>
        /// The expiration time of the access token in seconds.
        /// </summary>
        public int ExpiresIn { get; set; }
        /// <summary>
        /// The scopes contained in the access token.
        /// </summary>
        public string Scope { get; set; }
        /// <summary>
        /// An opaque refresh token. This is returned if the offline_access scope is granted
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
