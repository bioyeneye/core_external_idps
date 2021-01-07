using aux_oauth_server.service.DataAccess.Entities;
using aux_oauth_server.service.Managers.Models;
using aux_oauth_server.service.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using aux_oauth_server.service.Utilities;
using aux_oauth_server.service.Services;
using Microsoft.Extensions.Logging;

namespace aux_oauth_server.service.Managers.ProviderClients
{
    /// <summary>
    /// Google identity provider client manager, helps with getting access token from google token endpoint
    /// WellKnown Configuration - https://accounts.google.com/.well-known/openid-configuration
    /// </summary>
    public class GoogleIdentityProviderClient : BaseService<GoogleIdentityProviderClient>, IProviderManager
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public GoogleIdentityProviderClient(ILogger<GoogleIdentityProviderClient> logger) : base(logger)
        {
        }


        /// <summary>
        /// Get google access token from Google IDP
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="code"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public async Task<Response<StandardSuccessProviderResponses>> GetAccessTokenAsync(ProviderEntity entity, string code, string redirectUrl)
        {
            var response = Response<StandardSuccessProviderResponses>.Empty();
            try
            {
                try
                {
                    var responseFromGoogle = await entity.TokenEndpoint
                                    .PostUrlEncodedAsync(new
                                    {
                                        grant_type = "authorization_code",
                                        code = code,
                                        client_id = entity.ClientId,
                                        client_secret = entity.ClientSecret,
                                        redirect_uri = redirectUrl
                                    })
                                    .ReceiveJson<StandardSuccessProviderResponses>();

                    response = Response<StandardSuccessProviderResponses>.Success(responseFromGoogle);
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<StandardErrorProviderResponses>();
                    response = Response<StandardSuccessProviderResponses>.Failed($"{error.error_description}({error.error})");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetAccessTokenAsync)}");
                response = Response<StandardSuccessProviderResponses>.Failed("Error: Provider service is down");
            }

            return response;
        }
    }
}

/*var responseFromGoogle = await HttpHelper.MakeApplicationWwwFormUrlencodedAsync<StandardSuccessProviderResponses, StandardErrorProviderResponses>(
                   url: entity.TokenEndpoint,
                   data: new
                   {
                       grant_type = "authorization_code",
                       code = code,
                       //redirect_uri = entity.RedirectUrl.Split(',')[0],
                       client_id = entity.ClientId,
                       client_secret = entity.ClientSecret,
                       redirect_uri = redirectUrl
                   });*/
