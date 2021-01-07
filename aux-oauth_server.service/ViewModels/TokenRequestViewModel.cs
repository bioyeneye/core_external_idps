using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace aux_oauth_server.service.ViewModels
{
    /// <summary>
    /// View model for request for access token
    /// </summary>
    public class TokenRequestViewModel
    {
        /// <summary>
        /// Authorixation code from idp
        /// </summary>
        [Required]
        public string AuthorizationCode { get; set; }
        /// <summary>
        /// Client id fo
        /// </summary>
        [Required]
        public string ClientId { get; set; }
        /// <summary>
        /// Idp used to generate the token, like Google
        /// </summary>
        [Required]
        public string IdentityProvider { get; set; }

        /// <summary>
        /// Token request type
        /// </summary>
        public TokenRequestType TokenRequestType { get; set; }
        /// <summary>
        /// Redirect url
        /// </summary>
        public string RedirectUrl { get; set; }
    }

    /// <summary>
    /// Token request type : access token or refresh token
    /// </summary>
    public enum TokenRequestType
    {
        /// <summary>
        /// Access token type
        /// </summary>
        [Description("Access token")]
        ACCESS_TOKEN,

        /// <summary>
        /// Refresh token type
        /// </summary>
        [Description("Refresh token")]
        REFRESH_TOKEN
    }
}
