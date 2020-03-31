using CSAT.Services.Communication.DataCore.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSAT.Services.Communication.Data;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using CSAT.Services.Communication.DataCore.Data.Interfaces;
using System.Collections;
using CSAT.Services.Communication.DataCore;
using CSAT.Services.Communication.DataCore.Common;
using CSAT.Services.Communication.DataCore.Enums;

namespace FTDNA.Service.Genetics.Data
{
     public static class extensionclass  
        {
    public static List<T> MapToList<T>(this DbDataReader dr)
    {
        var objList = new List<T>();
        var props = typeof(T).GetProperties();

        var colMapping = dr.GetColumnSchema()
          .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
          .ToDictionary(key => key.ColumnName.ToLower());

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                T obj = Activator.CreateInstance<T>();
                foreach (var prop in props)
                {
                        try
                        {
                            var val =
                              dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                            prop.SetValue(obj, val == DBNull.Value ? null : val);
                        }
                        catch { }
                        //catch(Exception e )
                        //{
                        //    var error = e.Message; 
                        //}
                }
                objList.Add(obj);
            }
        }
        return objList;
    }
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }
        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue)
        {
            if (string.IsNullOrEmpty(cmd.CommandText))
                throw new InvalidOperationException(
                  "Call LoadStoredProc before using this method");
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
            return cmd;
        }
    }
    public class DataLayer : IDataLayer
    {
        public IConfiguration _configuration;
        public bool kitEncryptionOn = true;
        //public DataLayer( IConfiguration configuration) 
        //{
        //    this._configuration = configuration;

        //    this.CDN = _configuration.GetSection("CDN_WWW").Value;
        //}
        //private FTDNAContext _context;
       

        public static IEnumerable<ProfileFull> ExecuteReader<ProfileFull>(string query)
        {
            try
            {
                return ExecuteReader<ProfileFull>(query);
            }
            catch (Exception ex)
            {
                // your handling code here
                return null;
            }
        }
        public async Task<CommTypeListResults> getCommList(string dbconnection, string LapTopBarCodeID, int filterGroupID, string filterName, int offset, int pageSize)
        {

            CommTypeListResults results = new CommTypeListResults();
            using (CSATContext _context = new CSATContext(dbconnection))
            {
                try
                {
                   // var conn = _context.Database.GetDbConnection();

                   // conn.Open();
                    //using (var command = conn.CreateCommand())
                    //{
                        
                    //    //ADDING IN PARAMETERIZED QUERY
                    //    command.CommandType = CommandType.StoredProcedure;
                    //    command.CommandText = "sp_getCommTypes";

                    //    SqlParameter paramKitOwner = new SqlParameter("@LapTopBarCodeID", LapTopBarCodeID);
                    //    paramKitOwner.Direction = ParameterDirection.Input;
                    //    paramKitOwner.DbType = DbType.String;
                    //    command.Parameters.Add(paramKitOwner);
                    //    SqlParameter paramfilterGroupID = new SqlParameter("@filterGroupId", filterGroupID);
                    //    paramfilterGroupID.Direction = ParameterDirection.Input;
                    //    paramfilterGroupID.DbType = DbType.Int32;
                    //    command.Parameters.Add(paramfilterGroupID);
                    //    SqlParameter paramfilterName = new SqlParameter("@filtername", filterName);
                    //    paramfilterName.Direction = ParameterDirection.Input;
                    //    paramfilterName.DbType = DbType.String;
                    //    command.Parameters.Add(paramfilterName);
                    //    SqlParameter paramfilterOffset = new SqlParameter("@offset", offset);
                    //    paramfilterOffset.Direction = ParameterDirection.Input;
                    //    paramfilterOffset.DbType = DbType.Int32;
                    //    command.Parameters.Add(paramfilterOffset);
                    //    SqlParameter paramfilterPageSize = new SqlParameter("@pagesize", pageSize);
                    //    paramfilterPageSize.Direction = ParameterDirection.Input;
                    //    paramfilterPageSize.DbType = DbType.Int32;
                    //    command.Parameters.Add(paramfilterPageSize);

                        //END PARAMETERIZED QUERY

                        //DbDataReader readerCommMatches = await command.ExecuteReaderAsync();
                        //var commMatchData = readerCommMatches.MapToList<CommTypeList>();

                        //LETS SET CORRECT IMAGE PATH 
                        //foreach (CommTypeList i in commMatchData)
                        //{
                        //    results.totalMatches = i.totalMatches;
                        //    //DO MORE STUFF; 

                        //}
                        //results.Matches = commMatchData;
                        //results.totalMatches = commMatchData.Count();
                        //Leave in for new. This is for writing MockDataFile
                        // System.IO.File.WriteAllText(@"C:\projects\FTDNA.Services.GeneticInfo\FTDNA.Services.GeneticInfo.DataCore\MockData\FFmockjson_843.txt", writeFileValue);
                        //System.IO.File.WriteAllText(@"C:\projects\GeneticsSDK\FTDNA.GeneticInfo.SDK\FTDNA.Services.GeneticInfo.SDK\MockData\FFmockjson_843.txt", writeFileValue);

                        // readerCommMatches.Close();

                        //FAKE RETURN FOR TESTING 
                        List<CommTypeList> fakedReturnData = new List<CommTypeList>();
                        CommTypeList innerObject = new CommTypeList();

                        innerObject.messageType = CSAT.Services.Communication.DataCore.Enums.MessageType.ProductArrival;
                        innerObject.Message = "Hello CSAT world";
                        innerObject.name = "Test 1";
                        fakedReturnData.Add(innerObject);

                        results.Matches = fakedReturnData;
                        results.totalMatches = fakedReturnData.Count();

                        return results; 

                   // }
                }
                catch (Exception e)
                {
                    //TODO 
                    //HAND EXCEPTION CORRECTLY 
                    //return StatusCode((int)HttpStatusCode.InternalServerError, e);
                    return null;
                }

            }

            return results;
        }



    }
    
}
