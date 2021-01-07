using aux_oauth_server.service.Filters;
using aux_oauth_server.service.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace aux_oauth_server.service
{
    /// <summary>
    /// Common service configuration for service projects
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Helps with api configuration like making url lowercase, date formating, model state validation 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationApiConfiguration(this IServiceCollection services)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1
            //services.AddControllers(options => { options.Filters.Add(typeof(ValidateModelStateActionFilter)); });
            services.AddControllers(options =>
                {
                    // requires using Microsoft.AspNetCore.Mvc.Formatters;
                    options.OutputFormatters.RemoveType<StringOutputFormatter>();
                    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                    options.Filters.Add(typeof(ValidateModelStateActionFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    // Use the default property (Pascal) casing
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
                })
                .ConfigureApiBehaviorOptions(options =>
                {

                });
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            return services;
        }

        /// <summary>
        /// Configure swaager for api project
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="version">Version</param>
        /// <param name="title">Documentation Title</param>
        /// <param name="description">Documentation description</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, IConfiguration configuration, string version, string title, string description)
        {
            var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            services.AddSwaggerGen(c =>
            {
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    License = new OpenApiLicense
                    {
                        Name = "Microsoft Licence",
                        Url = new Uri("https://automedsys.net/licence"),
                    },
                    Description = description
                });
                c.SchemaFilter<EnumSchemaFilter>();
                //c.DescribeAllEnumsAsStrings();
                c.AddSecurityDefinition(appSettings.Scheme, new OpenApiSecurityScheme
                {
                    Description = @$"JWT Authorization header using the Bearer scheme. 
                      {Environment.NewLine}Enter 'Bearer' [space] and then your token in the text input below.
                      {Environment.NewLine}Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = appSettings.Scheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = appSettings.Scheme
                            },
                            Scheme = "oauth2",
                            Name = appSettings.Scheme,
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        /// <summary>
        /// Service configuration for authentication
        /// </summary>
        /// <param name="services">Service collection(core)</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="serviceLevel">Flag for token signing validation</param>
        /// <returns></returns>
        //public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration, bool serviceLevel = false)
        //{
        //    var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //    IdentityModelEventSource.ShowPII = true;
        //    services.AddAuthentication(sharedOptions =>
        //    {
        //        sharedOptions.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        //        sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //    {
        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true;
        //        options.Configuration = new OpenIdConnectConfiguration();
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = appSettings.ValidateIssuer,
        //            ValidateAudience = appSettings.ValidateAudience,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = serviceLevel ? false : appSettings.ValidateIssuerSigningKey,
        //            ValidIssuer = appSettings.Issuer,
        //            ValidAudience = appSettings.Audience,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
        //            ClockSkew = TimeSpan.Zero,
        //        };
        //        options.Events = new JwtBearerEvents
        //        {
        //            OnAuthenticationFailed = context =>
        //            {
        //                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
        //                return Task.CompletedTask;
        //            },
        //            OnTokenValidated = context =>
        //            {
        //                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
        //                return Task.CompletedTask;
        //            },
        //        };
        //    });

        //    return services;
        //}

        /// <summary>
        /// Health check service configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="name">Service name for health recognition</param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationHealthCheck(this IServiceCollection services, IConfiguration configuration, string name)
        {
            //var dbConfigString = configuration.GetConnectionString("Default");
            services.AddHealthChecks();
            //.AddSqlServer(dbConfigString);

            services.AddHealthChecksUI(setup =>
            {
                setup.AddHealthCheckEndpoint(name, "/health");
                setup.MaximumHistoryEntriesPerEndpoint(50);
            })
            .AddSqliteStorage("Data Source=health.db");

            return services;
        }
    }
}
