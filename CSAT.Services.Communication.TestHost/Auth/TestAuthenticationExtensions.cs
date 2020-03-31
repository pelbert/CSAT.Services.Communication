using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace CSAT.Services.Communication.TestHost.Auth
{
    public static class TestAuthenticationExtensions
    {
        public const string TEST_AUTH_SCHEME = "Test Scheme";
        private const string TEST_AUTH_DISPLAY_NAME = "Test Auth";

        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TEST_AUTH_SCHEME, TEST_AUTH_DISPLAY_NAME, configureOptions);
        }
    }
}
