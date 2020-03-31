using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using CSAT.Services.Communication.Web.Core;
namespace CSAT.Services.Communication.TestHost
{
    public abstract class HostBase<T>
            where T : Startup
    {
        protected readonly IWebHost Server;

        protected static IConfiguration Configuration { get; private set; } = ConfigurationFactory.GetConfiguration(AppDomain.CurrentDomain.BaseDirectory);

        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Base class for Integration testing. Provides a TestServer and a mechanism to override config settings
        /// for specific test classes
        /// </summary>
        /// <param name="outputHelper"></param>
        public HostBase(string[] configSettingOverrides)
        {
            configSettingOverrides = configSettingOverrides ?? new string[] { };

            // Validate environment variable.  If not set, set to Development.
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentName.Development);

            this.Server = WebHost.CreateDefaultBuilder(configSettingOverrides)
                                            .UseEnvironment(EnvironmentName.Development)
                                            .ConfigureAppConfiguration((context, builder) => ConfigurationFactory.UseDefaultConfig(context, builder, configSettingOverrides))
                                            .UseApplicationInsights()
                                            .UseStartup<T>()
                                            .UseKestrel()
                                            .Build();

            ServiceProvider = BypassAuthStartup.ServiceProvider;
        }
    }
}
