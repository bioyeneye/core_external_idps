using aux_oauth_server.service.DataAccess.Entities;
using aux_oauth_server.service.DataAccess.Repositories;
using aux_oauth_server.service.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace aux_oauth_server.service.Services
{
    /// <summary>
    /// Provider service interface
    /// </summary>
    public interface IProviderService
    {
        /// <summary>
        /// Get provider by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Response<ProviderResponse> GetProviderByClientId(string clientId);
        /// <summary>
        /// Get provider by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Response<ProviderResponse> GetProviderById(string clientId);
        /// <summary>
        /// Get provider by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Response<ProviderResponse> GetProviderByName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Response<List<ProviderResponse>> GetProviders();
    }

    /// <summary>
    /// Provider service, helps provider actions like get providers, add, update, delete
    /// </summary>
    public class ProviderService : BaseService<ProviderService>, IProviderService
    {
        private IProviderRepository providerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="providerRepository"></param>
        public ProviderService(ILogger<ProviderService> logger, IProviderRepository providerRepository) : base(logger)
        {
            this.providerRepository = providerRepository;
        }

        /// <summary>
        /// Get the providers from the db
        /// </summary>
        /// <returns></returns>
        public Response<List<ProviderResponse>> GetProviders()
        {
            try
            {
                var response = this.providerRepository.GetProviders();
                return Response<List<ProviderResponse>>.Success(response.Select(c=> MapEntityToResponse(c)).ToList());

            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetProviders)}");
                return Response<List<ProviderResponse>>.Failed("Error: Provider service is down");
            }
        }

        /// <summary>
        /// Get provider by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Response<ProviderResponse> GetProviderByName(string name)
        {
            try
            {
                var response = this.providerRepository.GetProviderByName(name);
                if (response == null)
                {
                    return Response<ProviderResponse>.NotFound($"Error: Provider with name {name} is not a valid provider registered on the system");
                }

                return Response<ProviderResponse>.Success(MapEntityToResponse(response));

            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetProviders)}");
                return Response<ProviderResponse>.Failed("Error: Provider service is down");
            }
        }

        /// <summary>
        /// Get provider by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<ProviderResponse> GetProviderByClientId(string clientId)
        {
            try
            {
                var response = this.providerRepository.GetProviderByClientId(clientId);
                if (response == null)
                {
                    return Response<ProviderResponse>.NotFound($"Error: Provider with Client Id {clientId} is not a valid provider registered on the system");
                }

                return Response<ProviderResponse>.Success(MapEntityToResponse(response));

            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetProviders)}");
                return Response<ProviderResponse>.Failed("Error: Provider service is down");
            }
        }


        /// <summary>
        /// Get provider by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<ProviderResponse> GetProviderById(string clientId)
        {
            try
            {
                var response = this.providerRepository.GetProviderById(clientId);
                if (response == null)
                {
                    return Response<ProviderResponse>.NotFound($"Error: Provider with id {clientId} is not a valid provider registered on the system");
                }

                return Response<ProviderResponse>.Success(MapEntityToResponse(response));

            }
            catch (Exception ex)
            {
                LogError(ex, $"Error from {nameof(GetProviders)}");
                return Response<ProviderResponse>.Failed("Error: Provider service is down");
            }
        }

        #region Helpers
        /// <summary>
        /// Helps map provider from entity to model
        /// </summary>
        /// <param name="providerEntity">Provider entityy from db or source</param>
        /// <returns></returns>
        private ProviderResponse MapEntityToResponse(ProviderEntity providerEntity)
        {
            return new ProviderResponse
            {
                Id = providerEntity.Id.ToString(),
                Name = providerEntity.Name,
                ClientId = providerEntity.ClientId,
                Secret = providerEntity.ClientSecret,
                Logo = providerEntity.Logo,
                Scope = providerEntity.ScopesSupported.Split(','),
                RedirectUrl = providerEntity.RedirectUrl.Split(','),
                GrantTypeAllowed = providerEntity.GrantTypesSupported.Split(','),
                ClientGrantUrls = new List<ClientGrantUrl>{
                            new ClientGrantUrl
                            {
                                GrantType = "authorization_code",
                                Url = @$"{providerEntity.AuthorizationEndpoint}?" +
                                        "access_type=offline" +
                                        "&include_granted_scopes=true" +
                                        $"&client_id={providerEntity.ClientId}" +
                                        "&scope=openid email profile" +
                                        "&response_type=code" +
                                        "&redirect_uri={redirect_uri}" +
                                        $"&state=google|{providerEntity.ClientId}"
                            },
                            new ClientGrantUrl
                            {
                                GrantType = "refresh_token",
                                Url = providerEntity.TokenEndpoint
                            },
                        }.ToArray(),
            };
        }
        #endregion

    }


}
