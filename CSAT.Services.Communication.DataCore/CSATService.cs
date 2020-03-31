using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CSAT.Services.Communication.DataCore.Data.Interfaces;
using CSAT.Services.Communication.DataCore.Models;

namespace CSAT.Services.Communication.DataCore
{
    public class CSATService
    {
        private IDataLayer dataLayer { get; }
        private string dbconnection;
        private bool CDNEnabled;
        private string CDN;
        private string LapTopBarcodeID;
        public CSATService(IDataLayer _dataLayer, string connection, string LapTopBarcodeID)
        {
            this.dataLayer = _dataLayer;
            this.dbconnection = connection;
            this.LapTopBarcodeID = LapTopBarcodeID;

        }
        public async Task<CommTypeListResults> GetCommunicationTypeList(   int filterGroupID, string filterName, int offset, int pagesize)
        {
            CommTypeListResults retval = await dataLayer.getCommList(this.dbconnection, this.LapTopBarcodeID,filterGroupID, filterName,  offset,  pagesize);
            return retval;
        }
    }
}
