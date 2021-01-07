using aux_oauth_server.service.DataAccess.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using aux_oauth_server.service.DataAccess.Repositories.core;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace aux_oauth_server.service.DataAccess.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProviderRepository
    {
        /// <summary>
        /// Get provider by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ProviderEntity GetProviderByClientId(string clientId);
        /// <summary>
        /// Get provider by provider id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProviderEntity GetProviderById(string id);
        /// <summary>
        /// Get provider by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ProviderEntity GetProviderByName(string name);

        /// <summary>
        /// Get all the registered providers
        /// </summary>
        /// <returns></returns>
        List<ProviderEntity> GetProviders(bool? isActive = null);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProviderRepository : IProviderRepository
    {
        private IDapperCore dapperCore;
        private IConfiguration configuration;
        private string Connectionstring = "DefaultConnection";

        /// <summary>
        /// Default provider repository constructor
        /// </summary>
        public ProviderRepository(IDapperCore dapperCore, IConfiguration configuration)
        {
            this.dapperCore = dapperCore;
            this.configuration = configuration;
        }

        private List<ProviderEntity> providers = new List<ProviderEntity>
            {
                new ProviderEntity
                {
                    Name = "Google",
                    Id = new Guid("73DEE486-FB87-40BD-BE80-622EE40395F8"),
                    ClientId = "276590763171-5lvfsoql497spassdmbccjkaqhofr7lp.apps.googleusercontent.com",
                    ClientSecret = "D5gc7izemeLoAkD9lVYGFINM",
                    Issuer = "https://accounts.google.com",
                    AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth",
                    DeviceAuthorizationEndpoint = "https://oauth2.googleapis.com/device/code",
                    TokenEndpoint = "https://oauth2.googleapis.com/token",
                    UserinfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo",
                    RevocationEndpoint = "https://oauth2.googleapis.com/revoke",
                    JwksUri = "https://www.googleapis.com/oauth2/v3/certs",
                    ClaimsSupported = "aud,email_verified,exp,family_name,given_name,iat,iss,locale,name,picture,sub",
                    GrantTypesSupported = "authorization_code,refresh_token",
                    ScopesSupported = "openid,email,profile",
                    IsActive = true,
                    Logo = "https://www.google.com/images/branding/googleg/1x/googleg_standard_color_128dp.png",
                    RedirectUrl = "https://pace.automedsys.net, https://qa-pace.automedsys.net, https://dev-pace.automedsys.net ,https://localhost, https://localhost:8235",
                }
            };

        /// <summary>
        /// Connection 
        /// </summary>
        /// <returns></returns>
        public DbConnection GetDbconnection()
        {
            var connection = new SqlConnection(configuration.GetConnectionString(Connectionstring));
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Get list of providers
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public List<ProviderEntity> GetProviders(bool? isActive = null)
        {
            var sql = "SELECT * FROM OAuthProviders";
            if (isActive.HasValue)
            {
                sql += isActive.Value ? " where IsActive = 1" : " where IsActive = 0";
            }
            return this.dapperCore.GetAll<ProviderEntity>(sql, null);
        }

        /// <summary>
        /// Get provider by name
        /// </summary>
        /// <returns></returns>
        public ProviderEntity GetProviderByName(String name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Name is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Name", name.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<ProviderEntity>("SELECT * FROM OAuthProviders where lower(Name) = @Name", parameters);
        }

        /// <summary>
        /// Get provider by id
        /// </summary>
        /// <returns></returns>
        public ProviderEntity GetProviderById(String id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new Exception("Id is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<ProviderEntity>("SELECT * FROM OAuthProviders where lower(Id) = @Id", parameters);
        }

        /// <summary>
        /// Get provider by client id
        /// </summary>
        /// <returns></returns>
        public ProviderEntity GetProviderByClientId(String clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new Exception("Client Id is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@ClientId", clientId.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<ProviderEntity>("SELECT * FROM OAuthProviders where lower(ClientId) = @ClientId", parameters);
        }
    }


}
