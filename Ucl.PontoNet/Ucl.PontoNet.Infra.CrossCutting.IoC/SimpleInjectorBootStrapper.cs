using System.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

using Ucl.PontoNet.Infra.Data.Context;

using Ucl.PontoNet.Infra.Data.UoW;
using Ucl.PontoNet.Domain.Base.Interfaces;
using Ucl.PontoNet.Domain.Repositories.Interfaces;
using Ucl.PontoNet.Application.Services;

using Ucl.PontoNet.Domain.Services;
using Ucl.PontoNet.Infra.Data.Repositories;
using Microsoft.Data.SqlClient;
using Ucl.PontoNet.Application.AutoMapper;
using Ucl.PontoNet.Application.Core.Services;
using Ucl.PontoNet.Domain.Core;
using Ucl.PontoNet.Domain.Core.Services;

namespace Ucl.PontoNet.Infra.CrossCutting.IoC
{
    public class SimpleInjectorBootStrapper
    {

        public Container container = new Container();

        private string ConnectionString { get; set; }

        public void InitializeContainer(IApplicationBuilder app, IConfiguration configuration)
        {

            container.Register(() => configuration, Lifestyle.Scoped);

            app.UseSimpleInjector(container);



            ConnectionString = configuration.GetConnectionString("DefaultConnection");

            // Add application presentation components:
            if (app != null)
            {
                //container.RegisterMvcControllers(app);
                //container.RegisterMvcViewComponents(app);

                // Cross-wire ASP.NET services (if any). For instance:
                //container.CrossWire<ILoggerFactory>(app);

                container.RegisterConditional(
                    typeof(ILogger),
                    c => typeof(Logger<>).MakeGenericType(c.Consumer.ImplementationType),
                    Lifestyle.Singleton,
                    c => true);
            }


            // Add application services. For instance:
            container.Register<IDbConnection>(() => new SqlConnection(ConnectionString), Lifestyle.Scoped);

            container.Register<IMemoryCache>(() => new MemoryCache(new MemoryCacheOptions() { SizeLimit = 5000 }), Lifestyle.Singleton);

            container.Register<DbContextOptions>(() =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                optionsBuilder.UseSqlServer(ConnectionString);
                return optionsBuilder.Options;
            }, Lifestyle.Scoped);

            container.Register<DbContext, DatabaseContext>(Lifestyle.Scoped);

            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);



            container.Register<IPersonSampleAppService, PersonSampleAppService>(Lifestyle.Scoped);
            container.Register<IPersonSampleService, PersonSampleService>(Lifestyle.Scoped);
            container.Register<IPersonSampleRepository, PersonSampleRepository>(Lifestyle.Scoped);
            
            container.Register<IClienteAppService, ClienteAppService>(Lifestyle.Scoped);
            container.Register<IClienteService, ClienteService>(Lifestyle.Scoped);
            container.Register<IClienteRepository, ClienteRepository>(Lifestyle.Scoped);
            
            container.Register<IFuncionarioAppService, FuncionarioAppService>(Lifestyle.Scoped);
            container.Register<IFuncionarioService, FuncionarioService>(Lifestyle.Scoped);
            container.Register<IFuncionarioRepository, FuncionarioRepository>(Lifestyle.Scoped);



            // container.RegisterSingleton(() => GetMapper(container));

            // container.RegisterSingleton<MapperConfiguration>(config);


            var config = AutoMapperConfig.RegisterMappings();
            container.RegisterInstance<MapperConfiguration>(config);
            container.Register<IMapper>(() => config.CreateMapper(container.GetInstance));



        }

        public void SetDefaultScopedLifestyle(ScopedLifestyle scope = null)
        {
            container.Options.DefaultScopedLifestyle = scope ?? new AsyncScopedLifestyle();
        }

        public void IntegrateSimpleInjector(IServiceCollection services)
        {
            //this.SetDefaultScopedLifestyle();
            //services.AddAutoMapper();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            // services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));


            //services.EnableSimpleInjectorCrossWiring(container);
            // services.UseSimpleInjectorAspNetRequestScoping(container);

            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope and
                // allows request-scoped framework services to be resolved.
                options.AddAspNetCore()

                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    // All calls are optional. You can enable what you need. For instance,
                    // ViewComponents, PageModels, and TagHelpers are not needed when you
                    // build a Web API.
                    .AddControllerActivation();

                // Optionally, allow application components to depend on the non-generic
                // ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
                // (Microsoft.Extensions.Localization) abstractions.
                options.AddLogging();
            });

            services.AddScoped<DatabaseContext>();
        }

        public void SetContainer(Container container)
        {
            this.container = container;
        }

        public void Verify()
        {
            container.Verify();
        }


    }
}