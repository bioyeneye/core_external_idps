using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace aux_oauth_server.service.Extensions
{
    /// <summary>
    /// Configure extension
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// configure security header for application
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConfigureSecurityHeaders(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseHsts(options => options.MaxAge(365).IncludeSubdomains());
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                await next();
            });

            app.UseHttpsRedirection();

            /*app.UseCsp(options =>
            {
                options.DefaultSources(directive => directive.Self());
                options.BlockAllMixedContent().FormActions(s => s.Self()).FrameAncestors(s => s.Self());
                options.ImageSources(directive => directive.Self().CustomSources("*"));
                options.ScriptSources(directive => directive.Self().UnsafeInline());
                options.StyleSources(directive => directive.Self().UnsafeInline());
            });*/

            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(options => options.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.Use((context, next) =>
            {
                if (context.Request.IsHttps) context.Response.Headers.Append("Expect-CT", "max-age=0; report-uri=\"https://automedsys.net/report-ct\"");
                return next.Invoke();
            });

            var forwardOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardOptions.KnownNetworks.Clear();
            forwardOptions.KnownProxies.Clear();
            app.UseForwardedHeaders(forwardOptions);

            return app;
        }

        /// <summary>
        /// Configure 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IApplicationBuilder ApplicationSwagger(this IApplicationBuilder app, string name)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", name);
                //c.DisplayOperationId();
                c.DisplayRequestDuration();

            });

            return app;
        }
    }
}
