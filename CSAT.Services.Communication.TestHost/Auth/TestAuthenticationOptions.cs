using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CSAT.Services.Communication.TestHost.Auth
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ClaimsIdentity Identity(HttpRequest req) => new ClaimsIdentity(new Claim[]
        {
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "test name id"),
            new Claim("encrypted_kit_id", req.Headers["Authorization"]),
        }, "test");
    }
}
