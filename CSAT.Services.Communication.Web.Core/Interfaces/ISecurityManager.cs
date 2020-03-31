using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSAT.Services.Communication.Web.Core.Interfaces
{
    public interface ISecurityManager
    {
        Security.Role? Role { get; set; }
    }
}
