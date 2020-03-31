using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CSAT.Services.Communication.Web.Core.Security;
using CSAT.Services.Communication.Data; 

namespace CSAT.Services.Communication.Web.Core
{
    public class DataConfig : Interfaces.IDataConfig
    {
        public string CSATDB { get; set; }
        
    }
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment env { get; set; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Console.WriteLine("entering");
            var startupfile = "appsettings.json";
            if (env.IsEnvironment("QA"))
            {
                Console.WriteLine("in QA");
                startupfile = "appsettings.QA.json";

            }
            else if (env.IsDevelopment())
            {

                Console.WriteLine("in Dev");
                startupfile = "appsettings.Development.json";
            }
            else if (env.IsStaging())
            {
                Console.WriteLine("in staging");
                startupfile = "appsettings.Staging.json";
            }
            Console.WriteLine("out ");
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile(startupfile, optional: true, reloadOnChange: true);

            Configuration = builder.Build();
            
        }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            ConfigureAuth(services);    
           

            services.AddTransient<Interfaces.ISecurityManager, SecurityManager>();
            services.AddTransient<Interfaces.IDataConfig, DataConfig>();
            services.Configure<DataConfig>(Configuration.GetSection("CSATDB"));
            services.AddMvc();
            services.AddDbContext<CSATContext>(option => option.UseSqlServer(Configuration.GetSection("CSATDB").Value));
            Console.WriteLine("Getting CSAT Config info");
            Console.WriteLine(Configuration.GetSection("CSATDB").Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
           this.ExposeServiceProvider(app.ApplicationServices);
        }
        protected virtual void ExposeServiceProvider(IServiceProvider serviceProvider) { }
        protected virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                const string Bearer = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultAuthenticateScheme = Bearer;
                options.DefaultChallengeScheme = Bearer;
            }).AddJwtBearer(opts =>
            {
                opts.Authority = Configuration["auth:authority"]; // Tenant
                opts.Audience = Configuration["auth:api_audience"];
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
            });
        }
    }
}
