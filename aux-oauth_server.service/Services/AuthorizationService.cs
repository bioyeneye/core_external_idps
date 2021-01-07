using aux_oauth_server.service.DataAccess.Entities;
using aux_oauth_server.service.DataAccess.Repositories;
using aux_oauth_server.service.Managers;
using aux_oauth_server.service.Responses;
using aux_oauth_server.service.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace aux_oauth_server.service.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Request access token from idp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<TokenResponse>> GetAccessTokenAsync(TokenRequestViewModel model);
    }

    /// <summary>
    /// Servvice that helps with authorization process like token request, refresh ton, token validation
    /// </summary>
    public class AuthorizationService : BaseService<AuthorizationService>, IAuthorizationService
    {
        private ProviderManager providerManager;
        private IRefreshTokenRepository refreshTokenRespository;

        /// <summary>
        /// Default constructor for the authorization service
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="providerManager"></param>
        /// <param name="refreshTokenRespository"></param>
        public AuthorizationService(ILogger<AuthorizationService> logger, ProviderManager providerManager, IRefreshTokenRepository refreshTokenRespository) : base(logger)
        {
            this.providerManager = providerManager;
            this.refreshTokenRespository = refreshTokenRespository;
        }

        /// <summary>
        /// Get access token from client, process the refresh token, get basic information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<TokenResponse>> GetAccessTokenAsync(TokenRequestViewModel model)
        {

            try
            {
                var response = await this.providerManager.GetAccessTokenFromProviderAsync(
                    name: model.IdentityProvider,
                    code: model.AuthorizationCode,
                    clientId: model.ClientId,
                    model.RedirectUrl);

                if (response.HasResult)
                {
                    //decode the token to jwt_token object
                    var tokenDetails = GetTokenDetails(response.ResponseData.IdToken);
                    //process the token for persitence
                    // update refresh token or insert
                    var processResponse = ProcessToken(response.ResponseData, tokenDetails);
                    var claims = tokenDetails.Claims;

                    var email = claims.FirstOrDefault(c => c.Type == "email").Value ?? "";
                    var picture = claims.FirstOrDefault(c => c.Type == "picture").Value ?? "";
                    var name = claims.FirstOrDefault(c => c.Type == "name").Value ?? "";
                    var given_name = claims.FirstOrDefault(c => c.Type == "given_name").Value ?? "";
                    var family_name = claims.FirstOrDefault(c => c.Type == "family_name").Value ?? "";

                    return Response<TokenResponse>.Success(new TokenResponse
                    {
                        AccessToken = response.ResponseData.AccessToken,
                        IdToken = response.ResponseData.IdToken,
                        RefreshToken = string.IsNullOrWhiteSpace(response.ResponseData.RefreshToken) ? processResponse : response.ResponseData.RefreshToken,
                        UserInformation = new TokenUserInformation
                        {
                            Picture = picture,
                            Email = email,
                            LastName = given_name,
                            FirstName = family_name,
                            StatusCode = 000,
                            MiscField1 = "47f8d1e56b7bda62a4a1524aeccd9964",
                            MiscField2 = "Practice Admin",
                            ErrorCode = "000",
                            Suggestion = "Login successful"
                        }
                    }); ; ;
                }

                return Response<TokenResponse>.Failed(response.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetAccessTokenAsync)}");
                return Response<TokenResponse>.Failed("Error: Token Service is down");
            }
        }

        #region HelperMethods

        /// <summary>
        /// Process token
        /// </summary>
        /// <param name="response">Provider token response</param>
        /// <param name="securityToken">Provider token deta</param>
        /// <returns></returns>
        private string ProcessToken(ProviderTokenResponse response, JwtSecurityToken securityToken)
        {
            var email = securityToken.Claims.FirstOrDefault(c => c.Type == "email").Value ?? "";
            var refreshTokenResponse = this.refreshTokenRespository.GetRefreshTokenByEmailAndProvider(email, securityToken.Issuer);
            if (string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                return refreshTokenResponse?.RefreshToken ?? "";
            }
            else
            {
                var entity = new TokenRefreshEntity
                {
                    RefreshToken = response.RefreshToken,
                    Provider = securityToken.Issuer,
                    Email = email,
                    DateUpdated = DateTime.Now
                };
                if (refreshTokenResponse == null)
                {
                    refreshTokenResponse = this.refreshTokenRespository.InsertRefreshToken(entity);
                    return entity.RefreshToken;
                }
                else
                {
                    entity.Id = refreshTokenResponse.Id;
                    refreshTokenResponse = this.refreshTokenRespository.UpdateRefreshToken(entity);
                    return entity.RefreshToken;
                }
            }
        }

        private JwtSecurityToken GetTokenDetails(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        #endregion
    }


}
