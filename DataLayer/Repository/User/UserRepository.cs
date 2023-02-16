using BusinessLayer.Users;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Auth;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public async Task< User> GetUser(long userid)
        {
            var data = await _sqlConnection.GetAsync<User>(userid, _transaction);
            return data;
        }

        public async Task<long> SubmitUser(User user)
        {
            try
            {
               // IDbTransaction dbTransaction = _sqlConnection.BeginTransaction();
                //_sqlConnection.Open();
                user.IsActive = true;
                user.IsDelete = false;
                user.Created_By = 0;
                user.Created_Date = DateTime.Now;
                user.Updated_By = 0;
                user.Updated_Date = DateTime.Now;
                var result = await _sqlConnection.InsertAsync<User>(user, _transaction);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public  async Task<bool> CheckUserExist(string username)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@Username", value: username);
           
          

            var data = (await _sqlConnection.QueryAsync<bool>("CheckExistsUser", param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure)).FirstOrDefault();

            return data;


        }

        public async Task<ValidUserDetailDC> GetUserDetailbyusername(string username)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@Username", value: username);
           
            var data = (await _sqlConnection.QueryAsync<ValidUserDetailDC>("GetDetailsByUserName", param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure)).FirstOrDefault();

            if (data == null)
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
