using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSAT.Services.Communication.TestHost.Auth
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(this.Options.Identity(this.Request)), new AuthenticationProperties(), TestAuthenticationExtensions.TEST_AUTH_SCHEME);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
