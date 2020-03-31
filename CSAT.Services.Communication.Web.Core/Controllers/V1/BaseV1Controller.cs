using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using CSAT.Services.Communication.Web.Core.Identity;
using CSAT.Services.Communication.Web.Core.Interfaces;
using CSAT.Services.Communication.Web.Core.Security;

namespace CSAT.Services.Communication.Web.Core.Controllers.V1
{
    public class BaseV1Controller : Controller
    {
        internal ISecurityManager SecurityManager { get; }
        public string Username { get; set; }
        public string KitNumber { get; set; }
        protected BaseV1Controller(ISecurityManager securityManager)
        {

             SecurityManager = securityManager;
        }
        protected void SetRequiredValues(bool forceNew = false)
        {
            //// Bypass for testing
            if (this.Username != null
                && this.KitNumber != null
                && this.SecurityManager.Role != null
                && !forceNew)
            {
                return;
            }
          
            var headers = this.Request.Headers;

            try
            {
                var tokenStr = ((string)headers["Authorization"]).Replace("Bearer ", string.Empty);
                var token = (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(tokenStr);

                if (token.Claims.All(x => x.Type != TokenConstants.Username))
                {
                    throw new ArgumentNullException(TokenConstants.Username, "Token does not contain user_name");
                }
                if (token.Claims.All(x => x.Type != TokenConstants.Role))
                {
                    throw new ArgumentNullException(TokenConstants.Role, "Token does not contain user_role");
                }
                if (token.Claims.All(x => x.Type != TokenConstants.KitNumber))
                {
                    throw new ArgumentNullException(TokenConstants.KitNumber, "Token does not contain kit_number");
                }

                this.SecurityManager.Role = (Role)Enum.Parse(typeof(Role), token.Claims.First(x => x.Type == TokenConstants.Role).Value);
                this.Username = token.Claims.First(x => x.Type == TokenConstants.Username).Value;
                this.KitNumber = token.Claims.First(x => x.Type == TokenConstants.KitNumber).Value;

                //Set RequestId from Header
                //if (!string.IsNullOrEmpty(headers["X-Request-Id"]))
                //{
                //    Guid id;
                //    this.RequestId = Guid.TryParse(headers["X-Request-Id"], out id) ? id : Guid.NewGuid();
                //}
                //else
                //{
                //    this.RequestId = new Guid();
                //}
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected bool UserNotAuthorized()
        {
            return SecurityManager.Role != Role.Editor
                     && SecurityManager.Role != Role.Owner
                     && SecurityManager.Role != Role.SysAdmin;
        }
    }
}