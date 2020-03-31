using System;

using System.Net;
using FTDNA.Service.Genetics.Data;

using CSAT.Services.Communication.DataCore.Models;
using CSAT.Services.Communication.DataCore.Data.Interfaces;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using CSAT.Services.Communication.Web.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;
using CSAT.Services.Communication.Data;
using CSAT.Services.Communication.DataCore;

namespace CSAT.Services.Communication.Web.Core.Controllers.V1
{
   // [Authorize]
    [Route("api/v1")]
    public class CommunicationController :  BaseV1Controller
    {
        //private readonly IOptions<MyConfig> config;
        public IConfiguration _configuration;
        public string connection;
        public bool CDNEnabled;
        private string CDN;
        private bool MOCKISTRUE;

        public CommunicationController(ISecurityManager securityManager, IConfiguration configuration) :
           base(securityManager)
        {
            this._configuration = configuration;
            this.connection = _configuration.GetSection("CSATDB").Value;
            this.MOCKISTRUE = Convert.ToBoolean(_configuration.GetSection("Mock").Value);

        }
        
       
        private DataLayer _dataLayer = new DataLayer();
        private DataLayerMock _dataLayerMock = new DataLayerMock();

        [HttpGet]
        [Route("CSAT/CommunicationsMessages/{barcodeid}/{offset}/{pagesize}")]  //Full profile with encryptedkit number 
        public async Task<IActionResult> GetFFMatches(string barcodeid, int filterGroupID, string filterName, int offset, int pagesize)
        {
            CommTypeListResults retval = null;
            try
            {
                if (MOCKISTRUE)
                {
                    //MOCK CALL 
                    CSATService communicationService = new CSATService(_dataLayerMock, this.connection, barcodeid);
                    retval = await communicationService.GetCommunicationTypeList( filterGroupID,  filterName,  offset,  pagesize);

       
                }
                else
                {
                    //NOT CURRENTLY BEING USED
                    //SetRequiredValues();
                    //if (UserNotAuthorized())
                    //{
                    //    return Unauthorized();
                    //}
                    //retval = _dataLayer.GetProfileFull(kitnumbers);
                    CSATService communicationService = new CSATService(_dataLayer, this.connection, barcodeid);
                    retval = await communicationService.GetCommunicationTypeList(filterGroupID, filterName, offset, pagesize);
                }
                return Json(retval);
            }
            catch (Exception e)
            {
                // log something
                // LogException(e);
                return StatusCode((int)HttpStatusCode.InternalServerError, e);
            }
        }



    }
}
