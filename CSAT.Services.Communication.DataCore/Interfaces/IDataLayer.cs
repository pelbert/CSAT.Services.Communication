
using CSAT.Services.Communication.DataCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAT.Services.Communication.DataCore.Data.Interfaces
{
    public interface IDataLayer
    {
               Task<CommTypeListResults> getCommList(string dbConnection,string lapTopBarCodeID,  int filterGroupID, string filterName, int offset, int pagesize);
     
    }
}
