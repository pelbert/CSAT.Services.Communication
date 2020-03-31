using Microsoft.AspNetCore.Hosting;


namespace CSAT.Services.Communication.TestHost
{
    public class BypassAuthHost : HostBase<BypassAuthStartup>
    {
        public BypassAuthHost(string[] args)
            : base(args)
        {
        }

        public static IWebHost Build(string[] args)
        {
            var host = new BypassAuthHost(args);
            return host.Server;
        }
    }
}
