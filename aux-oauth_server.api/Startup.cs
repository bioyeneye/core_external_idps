using aux_oauth_server.api.Middleware;
using aux_oauth_server.service;
using aux_oauth_server.service.Extensions;
using aux_oauth_server.service.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace aux_oauth_server.api
{
    /// <summary>
    /// /
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// /
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            IdentityModelEventSource.ShowPII = true;

            services.AddCors();
            services.AddApplicationService();
            services.AddApplicationApiConfiguration();
            services.AddSwaggerDoc(Configuration, "v1", "AutoMedSys OAuth Service", "Contents for Oauth services");
            services.AddApplicationHealthCheck(Configuration, nameof(aux_oauth_server));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestResponseLogging();
            app.UseConfigureSecurityHeaders(env);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.ApplicationSwagger("AutoMedSys OAuth Services");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
            });

            app.UseWelcomePage();
        }
    }
}
