using System;
using System.Collections.Generic;
using System.Text;

namespace CSAT.Services.Communication.DataCore.Models
{
    public class CommTypeListResults
    {
        public int totalMatches { get; set; }
        public IEnumerable<CommTypeList> Matches { get; set; }
    }
}
