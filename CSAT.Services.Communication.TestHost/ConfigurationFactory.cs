using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;


namespace CSAT.Services.Communication.TestHost
{
    /// <summary>
    /// Factory to build the IConfiguration for startup.
    /// This is pulled out so that Configuration can be used to setup logging before Dependency Injection and Application
    /// bootstrap.
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Configuration used during initial app startup, in order to pass Configuration to Logging startup.
        /// Also used in some tests.
        /// </summary>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(string basePath)
        {
            // Get environment
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Default to Development, if not set.
            if (string.IsNullOrEmpty(environment))
            {
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentName.Development);
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        /// <summary>
        /// Config builder used during startup of WebHost and TestWebHost
        /// </summary>
        /// <param name="hostingContext"></param>
        /// <param name="config"></param>
        /// <param name="args"></param>
        /// <exception cref="Exception"></exception>
        public static void UseDefaultConfig(WebHostBuilderContext hostingContext, IConfigurationBuilder config, string[] args)
        {
            var env = hostingContext.HostingEnvironment;
            var envName = env.EnvironmentName;

            // Fail fast when configuring environment for WebHostBuilder.  That should always be set by this point. If we get
            // here without an environment variable, setting it may be too late...
            if (string.IsNullOrEmpty(envName))
            {
                throw new Exception("Environment not found in WebHostBuilderContext. Ensure ASPNETCORE_ENVIRONMENT environment variable is set to a valid value (Development|Staging|Production).");
            }

            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true);
            config.AddEnvironmentVariables();
            config.AddCommandLine(args);
            
        }
    }
}
