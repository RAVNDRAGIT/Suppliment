using BusinessLayer;
using BusinessLayer.ProductMaster;
using Dapper;
using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Auth
{
    public class UserRepository : GenericRepository<User> ,IUserRepository
    {

        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public UserRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public ValidUserDetailDC Authentication(User user)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@username", value: user.UserName);
            dbArgs.Add(name: "@password", value: user.Password);
            var data = (_sqlConnection.Query<ValidUserDetailDC>("GetValidUserDetail", param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure)).FirstOrDefault();

            if (data==null)
            {
                return null;
            }
            else
            {
                return data;
                
            }
          
          
        }

        
    }
}
