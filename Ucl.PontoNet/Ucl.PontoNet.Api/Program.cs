using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ucl.PontoNet.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseSetting("detailedErrors", "true")
                .ConfigureAppConfiguration(ConfigConfiguration)
                .UseIISIntegration()

                .UseStartup<Startup>()
                .CaptureStartupErrors(true)
                .Build();
        }

        private static void ConfigConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder config)
        {
            config.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)

                .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
        }
    }
}
