using System;
using Xunit;
using CSAT.Services.Communication.DataCore;
using CSAT.Services.Communication.DataCore.Data;
using System.Linq;
using CSAT.Services.Communication.Data;

namespace ProfileServicesTests
{
    public class UnitTest1
    {
        private string connection = "";
        private bool CDNEnable = false;
        private string CDN = "";
        private string ownerKit = "9";
        private DataLayerMock _dataLayerMock = new DataLayerMock();
        //[Fact]
        //public async void SingleBasicGeneticsTest()
        //{
        //    string[] KitNumbers = { "9" };
        //    GeneticService returnBasicGenetics = new GeneticService(_dataLayerMock, this.connection, ownerKit);
        //    var retval = await returnBasicGenetics.GetGeneticsBasic(KitNumbers);
          
        //   // Xunit.Assert.Equal("Elliott Daniel Greenspan", retval.Succeded.ElementAt(0).FullName);
        //}

        //[Fact]
        //public async void FullGeneticsTest()
        //{
        //    string[] KitNumbers = { "9" };

        //    GeneticService returnFullGenetics = new GeneticService(_dataLayerMock, this.connection, ownerKit);
        //    var retval = await returnFullGenetics.GetGeneticsFull(KitNumbers);
        //  //  Xunit.Assert.Equal("mtFull Sequence", retval.Succeded.ElementAt(0).MtdnaTestTaken);

        //}

      

    }
}
