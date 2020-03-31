using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CSAT.Services.Communication.TestHost.Auth;
using System;
using CSAT.Services.Communication.Web.Core;

namespace CSAT.Services.Communication.TestHost
{
    public class BypassAuthStartup : Startup
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public BypassAuthStartup(IConfiguration configuration, IHostingEnvironment env) : base(configuration,env)
        {
        }

        protected override void ExposeServiceProvider(IServiceProvider serviceProvider)
        {
            BypassAuthStartup.ServiceProvider = serviceProvider;
        }

        protected override void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = TestAuthenticationExtensions.TEST_AUTH_SCHEME;
                options.DefaultChallengeScheme = TestAuthenticationExtensions.TEST_AUTH_SCHEME;
            })
            .AddTestAuth(o => { });


            services.AddAuthorization(options => {
            });
        }
    }
}
