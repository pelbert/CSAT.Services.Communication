using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoleType = CSAT.Services.Communication.Web.Core.Security.Role;



namespace CSAT.Services.Communication.Web.Core.Security
{
    public class SecurityManager : Interfaces.ISecurityManager
    {
        public RoleType? Role { get; set; }

        public SecurityManager() { }

        public SecurityManager(RoleType role)
        {
            Role = role;
        }

       
    }
}
