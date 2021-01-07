using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Responses
{
    /// <summary>
    /// Response send to client after success validation
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Id token
        /// </summary>
        public string IdToken { get; set; }
        /// <summary>
        /// Access token
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// User information
        /// </summary>
        public TokenUserInformation UserInformation { get; set; }
    }

    /// <summary>
    /// User information
    /// </summary>
    public class TokenUserInformation
    {
        /// <summary>
        /// User name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// user role
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        ///  user permission
        /// </summary>
        public string Permission { get; set; }
        public int StatusCode { get; internal set; }
        public string MiscField1 { get; internal set; }
        public string ErrorCode { get; internal set; }
        public string Suggestion { get; internal set; }
        public string MiscField2 { get; internal set; }
        public string Picture { get; internal set; }
    }
}
