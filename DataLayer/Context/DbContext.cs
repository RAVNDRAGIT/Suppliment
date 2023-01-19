using Dapper;
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
        private readonly string _connectionString;
       
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");

        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        public string AuthKey()
        {
            string _authKey = _configuration.GetSection("Key").GetSection("userKey").Value;
            return _authKey;
        }
    }
}

