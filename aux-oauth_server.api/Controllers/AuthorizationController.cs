using aux_oauth_server.service.Models;
using aux_oauth_server.service.Responses;
using aux_oauth_server.service.Services;
using aux_oauth_server.service.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aux_oauth_server.api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/connect")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class AuthorizationController : BaseApiController<AuthorizationController>
    {
        private IAuthorizationService authorizationService;
        /// <summary>
        /// Default authorization constructor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="logger"></param>
        /// <param name="authorizationService"></param>
        public AuthorizationController(IServiceProvider provider, ILogger<AuthorizationController> logger, IAuthorizationService authorizationService) : base(provider, logger)
        {
            this.authorizationService = authorizationService;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost("token")]
        [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Response<TokenResponse>), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Response<TokenResponse>>> PostAsync([FromBody] TokenRequestViewModel model)
        {
            var response = Response<TokenResponse>.Failed(Constants.UNAUTHORIZED_ERROR);

            try
            {
                switch (model.TokenRequestType)
                {
                    case TokenRequestType.ACCESS_TOKEN:
                        response = await authorizationService.GetAccessTokenAsync(model);
                        break;
                    case TokenRequestType.REFRESH_TOKEN:
                    default:
                        return BadRequest(Response<TokenResponse>.Failed("Grant type not implemented yet."));
                }


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
                LogError(ex, nameof(AuthorizationController), nameof(PostAsync));
                response.Message = Constants.SERVICE_NOT_AVAILABLE;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

        }
    }
}
