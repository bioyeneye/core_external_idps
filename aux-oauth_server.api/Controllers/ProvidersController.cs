using aux_oauth_server.service.Models;
using aux_oauth_server.service.Responses;
using aux_oauth_server.service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aux_oauth_server.api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class ProvidersController : BaseApiController<ProvidersController>
    {
        private IProviderService providerService;

        /// <summary>
        /// Provider default controller with logger and provider service
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="providerService"></param>
        public ProvidersController(IServiceProvider provider, ILogger<ProvidersController> logger, IProviderService providerService) : base(provider, logger)
        {
            this.providerService = providerService;
        }

        /// <summary>
        /// Resources to get list of providers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Response<List<ProviderResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<List<ProviderResponse>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<List<ProviderResponse>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<List<ProviderResponse>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<List<ProviderResponse>>), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public ActionResult<Response<List<ProviderResponse>>> Get()
        {
            var response = Response<List<ProviderResponse>>.Failed(Constants.UNAUTHORIZED_ERROR);

            try
            {
                /* var currentUser = GetCurrentUser();
                 if (currentUser == null)
                 {
                     return Unauthorized(response);
                 }*/

                response = providerService.GetProviders();
                if (response.Successful)
                {
                    return Ok(response);
                }

                if (response.ResultType == ResultType.NotFound)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(ProvidersController), nameof(Get));
                response.Message = Constants.SERVICE_NOT_AVAILABLE;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Resources to get a provider
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<ProviderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ProviderResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ProviderResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<ProviderResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<ProviderResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public ActionResult<Response<ProviderResponse>> GetById([FromRoute] string id)
        {
            var response = Response<ProviderResponse>.Failed(Constants.UNAUTHORIZED_ERROR);

            try
            {
                /* var currentUser = GetCurrentUser();
                 if (currentUser == null)
                 {
                     return Unauthorized(response);
                 }*/

                response = providerService.GetProviderById(id);
                if (response.Successful)
                {
                    return Ok(response);
                }

                if (response.ResultType == ResultType.NotFound)
                {
                    return NotFound(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(ProvidersController), nameof(Get));
                response.Message = Constants.SERVICE_NOT_AVAILABLE;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
