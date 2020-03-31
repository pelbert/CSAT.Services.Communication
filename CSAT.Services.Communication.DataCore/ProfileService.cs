using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FTDNA.Service.Genetics.Data.Models;
using Microsoft.Extensions.Configuration;

namespace FTDNA.Service.Genetics.Data
{
    public class ProfileService
    {

        private Interfaces.IDataLayer dataLayer { get; }
        private string dbconnection;
        private bool CDNEnabled;
        private string CDN;
        public ProfileService(Interfaces.IDataLayer _dataLayer, string connection, bool CDNEnabled, string CDN)
        {
            this.dataLayer = _dataLayer;
            this.dbconnection = connection;
            this.CDNEnabled = CDNEnabled;
            this.CDN = CDN;
        }
        public async Task<ReturnBasicProfile> GetProfileBasic(string[] kitnumbers)
        {
            ReturnBasicProfile retval =await  dataLayer.GetProfileBasic(kitnumbers, dbconnection,CDNEnabled, CDN);
            return retval;
        }

        public ReturnFullProfile GetProfileFull(string[] kitnumber)
        {
            ReturnFullProfile retval = dataLayer.GetProfileFull(kitnumber, dbconnection, CDNEnabled, CDN);
            return retval;
        }
    }
}
