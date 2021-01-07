using System;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.DataAccess.Entities
{
    /// <summary>
    /// Token and refresh entity
    /// </summary>
    public class TokenRefreshEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Provider { get; set; }
        ///  <summary>
        ///  Email address of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Date the refesh token was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Date updated
        /// </summary>
        public DateTime DateUpdated { get; set; }
    }
}