using System;
using Microsoft.AspNetCore.Hosting;

namespace CSAT.Services.Communication.TestHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => BypassAuthHost.Build(args);
    }
}
