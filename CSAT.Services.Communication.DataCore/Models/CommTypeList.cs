using System;
using System.Collections.Generic;
using System.Text;

namespace CSAT.Services.Communication.DataCore.Models
{
    public class CommTypeList
    {

        public int totalMatches { get; set; }
        public string Message { get; set; }
        public string name { get; set; }
        public int bucket { get; set; }
        public Enums.MessageType messageType { get; set; }

    }

    
}
