using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration; 
using System.Configuration;

namespace CSAT.Services.Communication.Data
{
    public class CSATContext : DbContext
    {
        private string dbconnection;
        //public IConfiguration _configuration;
        //public string connection2;
        //public CSATContext(IConfiguration configuration) 
        //{
        //    this._configuration = configuration;
        //    this.connection = _configuration.GetSection("DBSTRING").Value;
        //}
        //public CSATContext(DbContextOptions<FTDNAContext> options)
        //    : base(options)
        //{ }
        public CSATContext(string connection )
        {
            this.dbconnection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
              //  var myKey1 = Configuration["DBSTRING"];
               // var myKey1 = configuration["DBSTRING"];
               // var connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                //this.connection2 = configuration.GetSection("DBSTRING").Value;
            }
            catch(Exception e)
            {
                var message = e.Message; 
            }
            
         optionsBuilder.UseSqlServer(dbconnection);
           

        }
      
    }


  

}
