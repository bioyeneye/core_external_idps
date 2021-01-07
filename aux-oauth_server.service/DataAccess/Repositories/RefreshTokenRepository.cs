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
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        TokenRefreshEntity GetRefreshTokenByEmail(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        TokenRefreshEntity GetRefreshTokenByToken(string refreshToken);
        TokenRefreshEntity GetRefreshTokenByTokenAndProvider(string refreshToken, string provider);
        TokenRefreshEntity GetRefreshTokenByEmailAndProvider(string refreshToken, string provider);
        TokenRefreshEntity InsertRefreshToken(TokenRefreshEntity refreshToken);
        TokenRefreshEntity UpdateRefreshToken(TokenRefreshEntity refreshToken);
    }

    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private IDapperCore dapperCore;
        private IConfiguration configuration;
        private string Connectionstring = "DefaultConnection";

        /// <summary>
        /// Default provider repository constructor
        /// </summary>
        public RefreshTokenRepository(IDapperCore dapperCore, IConfiguration configuration)
        {
            this.dapperCore = dapperCore;
            this.configuration = configuration;
        }

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
        /// Get refresh token by email
        /// </summary>
        /// <returns></returns>
        public TokenRefreshEntity GetRefreshTokenByEmail(String email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Name is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<TokenRefreshEntity>("SELECT * FROM OAuthRefreshTokens where lower(Email) = @Email", parameters);
        }

        /// <summary>
        /// Get refresh token by email and provider
        /// </summary>
        /// <returns></returns>
        public TokenRefreshEntity GetRefreshTokenByTokenAndProvider(String refreshToken, String provider)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("RefreshToken is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@RefreshToken", refreshToken.ToLower(), DbType.String, ParameterDirection.Input);
            parameters.Add("@Provider", provider.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<TokenRefreshEntity>("SELECT * FROM OAuthRefreshTokens WHERE lower(RefreshToken) = @RefreshToken AND lower(Provider) = @Provider", parameters);
        }

        /// <summary>
        /// Get refresh token entity using email and issuer
        /// </summary>
        /// <param name="email"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public TokenRefreshEntity GetRefreshTokenByEmailAndProvider(String email, String provider)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email.ToLower(), DbType.String, ParameterDirection.Input);
            parameters.Add("@Provider", provider.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<TokenRefreshEntity>("SELECT * FROM OAuthRefreshTokens WHERE lower(Email) = @Email AND lower(Provider) = @Provider", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public TokenRefreshEntity GetRefreshTokenByToken(String refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new Exception("RefreshToken is null");
            }

            var parameters = new DynamicParameters();
            parameters.Add("@RefreshToken", refreshToken.ToLower(), DbType.String, ParameterDirection.Input);
            return this.dapperCore.Get<TokenRefreshEntity>("SELECT * FROM OAuthRefreshTokens where lower(RefreshToken) = @RefreshToken", parameters);
        }

        /// <summary>
        /// Insert refresh token
        /// </summary>
        /// <param name="refreshToken">Entity</param>
        /// <returns></returns>
        public TokenRefreshEntity InsertRefreshToken(TokenRefreshEntity refreshToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RefreshToken", refreshToken.RefreshToken, DbType.String, ParameterDirection.Input);
            parameters.Add("@Provider", refreshToken.Provider, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", refreshToken.Email, DbType.String, ParameterDirection.Input);
            var sq = @"INSERT INTO [dbo].[OAuthRefreshTokens]
                       ([Provider]
                       ,[RefreshToken]
                       ,[Email])
                        VALUES
                       (@Provider, @RefreshToken, @Email)";
            return this.dapperCore.Insert<TokenRefreshEntity>(sq, parameters);
        }

        /// <summary>
        /// Insert refresh token
        /// </summary>
        /// <param name="refreshToken">Entity</param>
        /// <returns></returns>
        public TokenRefreshEntity UpdateRefreshToken(TokenRefreshEntity refreshToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RefreshToken", refreshToken.RefreshToken, DbType.String, ParameterDirection.Input);
            parameters.Add("@Provider", refreshToken.Provider, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", refreshToken.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Id", refreshToken.Id, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@DateUpdated", refreshToken.DateUpdated, DbType.DateTime, ParameterDirection.Input);
            var sq = @"UPDATE [dbo].[OAuthRefreshTokens]
                       SET [Provider] = @Provider
                      ,[RefreshToken] = @RefreshToken
                      ,[Email] = @Email
                      ,[DateUpdated] = @DateUpdated
                        WHERE Id = @Id";
            return this.dapperCore.Update<TokenRefreshEntity>(sq, parameters);
        }
    }


}
