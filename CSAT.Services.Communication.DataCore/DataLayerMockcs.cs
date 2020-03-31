using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSAT.Services.Communication.DataCore.Data.Interfaces;
using CSAT.Services.Communication.DataCore.Models;
using Newtonsoft.Json;

namespace CSAT.Services.Communication.Data
{
    public class DataLayerMock : IDataLayer
    {

       
        public async Task<CommTypeListResults> getCommList(string connection, string LapTopBarCode, int filterGroupID, string filterName, int offSet, int pageSize)
        {

            //READ FROM MOCK DATA FILES FOR TESTING FROM SDK TO BYPASS DATA ACCESS. 
            try
            {
                List<CommTypeList> retval;
                IEnumerable<CommTypeList> retvalMock;
                CommTypeListResults queryitems = new CommTypeListResults();
                var resourceName = "";

                var assembly = Assembly.GetExecutingAssembly();
                if (LapTopBarCode == "1")
                {
                    resourceName = "CSAT.Services.Communication.DataCore.MockData.floorStageComm1.txt";
                }
                else if (LapTopBarCode == "2")
                {
                    resourceName = "CSAT.Services.Communication.DataCore.MockData.floorStageComm2.txt";

                }
                else if (LapTopBarCode == "3")
                {
                    resourceName = "CSAT.Services.Communication.DataCore.MockData.floorStageComm3.txt";

                }
                else if (LapTopBarCode == "4")
                {
                    resourceName = "CSAT.Services.Communication.DataCore.MockData.floorStageComm4.txt";

                }
                 

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader r = new StreamReader(stream))
                {
                    string json = await r.ReadToEndAsync();
                    retval = JsonConvert.DeserializeObject<List<CommTypeList>>(json);
                    
                }

                if (filterGroupID >0)
                {
                    retvalMock = from c in retval
                                 where c.bucket == (filterGroupID)
                                 select c;
                }
                else
                {
                    retvalMock = retval;
                }
                if (filterName != null)
                {
                    retvalMock = from c in retvalMock
                                 where c.Message.ToUpper().Contains(filterName)
                                 select c;
                }
                //lets get total count prior to paging 
                queryitems.totalMatches = retval.Count();
                if (pageSize > 0)
                {
                    retvalMock = retvalMock.Skip(offSet).Take(pageSize);
                }
                queryitems.Matches = retvalMock;
                return queryitems;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
