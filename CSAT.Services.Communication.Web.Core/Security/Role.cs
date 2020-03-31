using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSAT.Services.Communication.Web.Core.Security
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        Reader,
        Related,
        Editor,
        Owner,
        SysAdmin
    }
}
