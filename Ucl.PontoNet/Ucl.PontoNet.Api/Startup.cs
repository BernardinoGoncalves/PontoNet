using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Ucl.PontoNet.Api.Filters;
using Ucl.PontoNet.Api.Provider;
using Ucl.PontoNet.Application.Dto;
using Ucl.PontoNet.Infra.CrossCutting.IoC;

using SimpleInjector;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging.AzureAppServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Ucl.PontoNet.Api.Policies;

namespace Ucl.PontoNet.Api
{
    public class Startup
    {
        public static readonly List<string> AuthenticatedEnvironments = new List<string>() { "production", "qa" };

        private readonly SimpleInjectorBootStrapper Injector = new SimpleInjectorBootStrapper();

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Injector.container.Options.ResolveUnregisteredConcreteTypes = false;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            JwtIssuerOptions options = app.ApplicationServices
                .GetService<IOptions<JwtIssuerOptions>>()
                .Value;


            Injector.InitializeContainer(app, Configuration);
            Injector.Verify();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors("ValePolicy");
            app.UseMiddleware(typeof(InterceptorHandlingMiddleware));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Ucl.PontoNet.Api API V1 - " + env.EnvironmentName);
            });

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();

            services.Configure<AzureFileLoggerOptions>(Configuration.GetSection("AzureLogging"));


            services.AddScoped<IMemoryCacheWithPolicy<UserInfoDto>, MemoryCacheWithPolicy<UserInfoDto>>();


            services.AddCors(options =>
            {
                if (!AuthenticatedEnvironments.Contains(environment))
                {

                    options.AddPolicy("ValePolicy",
                        builder =>
                        {
                            builder
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowAnyOrigin()
                                .WithExposedHeaders("X-Total-Count");
                        });




                }
                else
                {
                    options.AddPolicy("ValePolicy",
                        builder =>
                        {
                            builder
                                .WithExposedHeaders("X-Total-Count");
                        });
                }

            });

            services.ConfigureJwtServices(this.Configuration);


            string tokenAuthorizeIntegrationWs = Environment.GetEnvironmentVariable("IntegrationWs");

            services.AddAuthorization(options =>

            {

                options.AddPolicy("IntegrationWs", policy =>

                    policy.Requirements.Add(new AuthorizeAppRequirement(tokenAuthorizeIntegrationWs, environment)));

            });

            services.AddSingleton<IAuthorizationHandler, AuthorizeAppHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Ucl.PontoNet.Api API",
                        Description = "List of microservice to manage the entities",
                        Version = "v1",

                        Contact = new OpenApiContact
                        {
                            Name = "Digital Solutions",
                            Email = "C0497842@vale.com"
                        },
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });


                c.DescribeAllEnumsAsStrings();
                c.OrderActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                c.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("ValePolicy"));
            //});

            var mvc = services.AddMvc(setupAction =>
            {
                if (!AuthenticatedEnvironments.Contains(environment))
                {
                    setupAction.Filters.Add(new AllowAnonymousFilter());
                }

                setupAction.Filters.Add(new ProducesAttribute("application/json", "application/xml"));

                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            mvc.AddJsonOptions(
                options =>
                        {


                            // options.JsonSerializerOptions.PropertyNamingPolicy = new CustomPropertyNamingPolicy();
                            options.JsonSerializerOptions.IgnoreNullValues = true;
                            options.JsonSerializerOptions.WriteIndented = false;
                            options.JsonSerializerOptions.AllowTrailingCommas = false;
                            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                        });


            Injector.IntegrateSimpleInjector(services);
            services.AddApplicationInsightsTelemetry(Configuration);
        }
    }
}