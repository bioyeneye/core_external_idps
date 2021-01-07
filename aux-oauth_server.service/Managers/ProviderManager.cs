using aux_oauth_server.service.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using aux_oauth_server.service.Services;
using Microsoft.Extensions.Logging;
using aux_oauth_server.service.DataAccess.Repositories;
using aux_oauth_server.service.Managers.Models;
using System.Threading.Tasks;
using Flurl.Http;

namespace aux_oauth_server.service.Managers
{
    /// <summary>
    /// Interfce for the provider clients
    /// </summary>
    public interface IProviderManager
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Response<StandardSuccessProviderResponses>> GetAccessTokenAsync(DataAccess.Entities.ProviderEntity entity, string code, string redirectUrl);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProviderManager : BaseService<ProviderManager>
    {
        private IServiceProvider _serviceProvider;
        private IProviderManager providerManager;
        private IProviderRepository providerRepository;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ProviderManager(ILogger<ProviderManager> logger, IServiceProvider serviceProvider, IProviderRepository providerRepository) : base(logger)
        {
            _serviceProvider = serviceProvider;
            this.providerRepository = providerRepository;
        }

        /// <summary>
        /// Get access token from provider by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Response<ProviderTokenResponse>> GetAccessTokenFromProviderAsync(string name, string code, string clientId, string redirectUrl)
        {
            try
            {
                var client = this.providerRepository.GetProviderByClientId(clientId);
                if (client == null)
                {
                    return Response<ProviderTokenResponse>.Failed("Invalid client id provider, kinldy request for configured providers");
                }

                try
                {
                    var resp = await client.TokenEndpoint
                                    .PostUrlEncodedAsync(new
                                    {
                                        grant_type = "authorization_code",
                                        code = code,
                                        client_id = client.ClientId,
                                        client_secret = client.ClientSecret,
                                        redirect_uri = redirectUrl
                                    })
                                    .ReceiveJson<StandardSuccessProviderResponses>();

                    return Response<ProviderTokenResponse>.Success(new ProviderTokenResponse
                    {
                        AccessToken = resp.access_token,
                        TokenType = resp.token_type,
                        ExpiresIn = resp.expires_in,
                        Scope = resp.scope,
                        IdToken = resp.id_token,
                        RefreshToken = resp.refresh_token
                    });
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<StandardErrorProviderResponses>();
                    return Response<ProviderTokenResponse>.Failed($"{error.error_description}({error.error})");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetAccessTokenFromProviderAsync)}");
                return Response<ProviderTokenResponse>.Failed("Error: Provider service is down");
            }
        }

        /// <summary>
        /// Get access token from provider by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Response<ProviderTokenResponse>> GetAccessTokenFromProviderUsingRFAsync(string name, string code, string clientId, string redirectUrl)
        {
            try
            {
                var client = this.providerRepository.GetProviderByClientId(clientId);
                if (client == null)
                {
                    return Response<ProviderTokenResponse>.Failed("Invalid client id provider, kinldy request for configured providers");
                }

                try
                {
                    var resp = await client.TokenEndpoint
                                    .PostUrlEncodedAsync(new
                                    {
                                        grant_type = "refresh_token",
                                        client_id = client.ClientId,
                                        client_secret = client.ClientSecret,
                                        refresh_token = code,
                                    })
                                    .ReceiveJson<StandardSuccessProviderResponses>();

                    return Response<ProviderTokenResponse>.Success(new ProviderTokenResponse
                    {
                        AccessToken = resp.access_token,
                        TokenType = resp.token_type,
                        ExpiresIn = resp.expires_in,
                        Scope = resp.scope,
                        IdToken = resp.id_token,
                        RefreshToken = resp.refresh_token
                    });
                }
                catch (FlurlHttpException ex)
                {
                    var error = await ex.GetResponseJsonAsync<StandardErrorProviderResponses>();
                    return Response<ProviderTokenResponse>.Failed($"{error.error_description}({error.error})");
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetAccessTokenFromProviderAsync)}");
                return Response<ProviderTokenResponse>.Failed("Error: Provider service is down");
            }
        }



        /*public async Task<Response<ProviderTokenResponse>> GetAccessTokenFromProviderAsyncOld(string name, string code, string clientId, string redirectUrl)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = _serviceProvider.GetService<IProviderManager>(name);
                    if (service == null)
                    {
                        return Response<ProviderTokenResponse>.Failed("Provider is not configured yet, kinldy request for configured providers");
                    }

                    var client = this.providerRepository.GetProviderByClientId(clientId);
                    if (client == null)
                    {
                        return Response<ProviderTokenResponse>.Failed("Invalid client id provider, kinldy request for configured providers");
                    }

                    var resp = await service.GetAccessTokenAsync(client, code, redirectUrl);
                    if (resp.Successful)
                    {
                        return Response<ProviderTokenResponse>.Success(new ProviderTokenResponse
                        {
                            AccessToken = resp.ResponseData.access_token,
                            TokenType = resp.ResponseData.token_type,
                            ExpiresIn = resp.ResponseData.expires_in,
                            Scope = resp.ResponseData.scope,
                            IdToken = resp.ResponseData.id_token,
                            RefreshToken = resp.ResponseData.refresh_token
                        });
                    }

                    return Response<ProviderTokenResponse>.Failed(resp.Message);
                }
            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetAccessTokenFromProviderAsync)}");
                return Response<ProviderTokenResponse>.Failed("Error: Provider service is down");
            }
        }
    */
    }
}


//get the provider details here

/*return Response<ProviderTokenResponse>.Success(new ProviderTokenResponse
{
    AccessToken = @"eyJhbGciOiJSUzI1NiJ9.eyJ2ZXIiOjEsImlzcyI6Imh0dHA6Ly9yYWluLm9rdGExLmNvbToxODAyIiwiaWF0IjoxNDQ5Nj
                      I0MDI2LCJleHAiOjE0NDk2Mjc2MjYsImp0aSI6IlVmU0lURzZCVVNfdHA3N21BTjJxIiwic2NvcGVzIjpbIm9wZW5pZCIsI
                      mVtYWlsIl0sImNsaWVudF9pZCI6InVBYXVub2ZXa2FESnh1a0NGZUJ4IiwidXNlcl9pZCI6IjAwdWlkNEJ4WHc2STZUVjRt
                      MGczIn0.HaBu5oQxdVCIvea88HPgr2O5evqZlCT4UXH4UKhJnZ5px-ArNRqwhxXWhHJisslswjPpMkx1IgrudQIjzGYbtLF
                      jrrg2ueiU5-YfmKuJuD6O2yPWGTsV7X6i7ABT6P-t8PRz_RNbk-U1GXWIEkNnEWbPqYDAm_Ofh7iW0Y8WDA5ez1jbtMvd-o
                      XMvJLctRiACrTMLJQ2e5HkbUFxgXQ_rFPNHJbNSUBDLqdi2rg_ND64DLRlXRY7hupNsvWGo0gF4WEUk8IZeaLjKw8UoIs-E
                      TEwJlAMcvkhoVVOsN5dPAaEKvbyvPC1hUGXb4uuThlwdD3ECJrtwgKqLqcWonNtiw",
    TokenType = "Bearer",
    ExpiresIn = 3600,
    Scope = "openid email",
    IdToken = @"a9VpZDRCeFh3Nkk2VdY",
    RefreshToken = @"eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiIwMHVpZDRCeFh3Nkk2VFY0bTBnMyIsImVtYWlsIjoid2VibWFzdGVyQGNsb3VkaXR1ZG
                  UubmV0IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsInZlciI6MSwiaXNzIjoiaHR0cDovL3JhaW4ub2t0YTEuY29tOjE4MDIiLCJsb
                  2dpbiI6ImFkbWluaXN0cmF0b3IxQGNsb3VkaXR1ZGUubmV0IiwiYXVkIjoidUFhdW5vZldrYURKeHVrQ0ZlQngiLCJpYXQiOjE0
                  NDk2MjQwMjYsImV4cCI6MTQ0OTYyNzYyNiwiYW1yIjpbInB3ZCJdLCJqdGkiOiI0ZUFXSk9DTUIzU1g4WGV3RGZWUiIsImF1dGh
                  fdGltZSI6MTQ0OTYyNDAyNiwiYXRfaGFzaCI6ImNwcUtmZFFBNWVIODkxRmY1b0pyX1EifQ.Btw6bUbZhRa89DsBb8KmL9rfhku
                  --_mbNC2pgC8yu8obJnwO12nFBepui9KzbpJhGM91PqJwi_AylE6rp-ehamfnUAO4JL14PkemF45Pn3u_6KKwxJnxcWxLvMuuis
                  nvIs7NScKpOAab6ayZU0VL8W6XAijQmnYTtMWQfSuaaR8rYOaWHrffh3OypvDdrQuYacbkT0csxdrayXfBG3UF5-ZAlhfch1fhF
                  T3yZFdWwzkSDc0BGygfiFyNhCezfyT454wbciSZgrA9ROeHkfPCaX7KCFO8GgQEkGRoQntFBNjluFhNLJIUkEFovEDlfuB4tv_M
                  8BM75celdy3jkpOurg"
});*/