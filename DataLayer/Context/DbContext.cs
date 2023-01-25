using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            

        }
        
        public string AuthKey()
        {
            string _authKey = _configuration.GetSection("Key").GetSection("userKey").Value;
            return _authKey;
        }

        public string MongoConString()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("ConnectionString").Value;
            return con;
        }

        public string MongoDbName()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("DatabaseName").Value;
            return con;
        }

        public string MongoOrderCollection()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("OrderCollectionName").Value;
            return con;
        }
       
        private void ResetRepositories()
        {

        }

        
        
    }
}

