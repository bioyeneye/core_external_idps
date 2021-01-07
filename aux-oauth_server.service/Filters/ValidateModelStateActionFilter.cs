using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using aux_oauth_server.service.Responses;

namespace aux_oauth_server.service.Filters
{
    /// <summary>
    /// Validation error filter
    /// </summary>
    public class ValidateModelStateActionFilter : IAsyncActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Check if ModelState is valid.
            if (!context.ModelState.IsValid)
            {
                var validationError = context?.ModelState?
                                .Keys
                                .Where(i => context.ModelState[i].Errors.Count > 0)?
                                .Select(k => context.ModelState[k]?.Errors?.First()?.ErrorMessage)?
                                .ToList();
                var result = new BadRequestObjectResult(Response.ValidationError(validationError));

                result.ContentTypes.Add(MediaTypeNames.Application.Json);
                result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                context.Result = result;
            }
            else
            {
                await next();
            }
        }
    }
}
