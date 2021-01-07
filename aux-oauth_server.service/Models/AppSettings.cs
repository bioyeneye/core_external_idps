namespace aux_oauth_server.service.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateIssuer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateLifetime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateAudience { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Scheme { get; set; }

        //todo: include cors implementations
    }
}
