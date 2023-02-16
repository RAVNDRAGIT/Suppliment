using BusinessLayer.Users;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Address;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Address
{
    public class UserLocationRepository : GenericRepository<UserLocationRepository>, IUserLocationRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public UserLocationRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<long> SubmitUserLocation(UserLocation userLocations, long userid)
        {
            userLocations.Created_By = userid;
            userLocations.Created_Date = DateTime.Now;
            userLocations.Updated_By = userid;
            userLocations.Updated_Date = DateTime.Now;
            userLocations.IsActive = true;
            userLocations.IsDelete = false;
            userLocations.IsDefault = true;
            var data = await _sqlConnection.InsertAsync<UserLocation>(userLocations, _transaction);
                return data;
        }

        public async Task<UserLocation> GetById(long id)
        {
           var data = await _sqlConnection.GetAsync<UserLocation>(id,_transaction);
            return data;
        }

        public async Task<List<UserLocationResultDC>> GetAddressbyUserId(long userid)
        {
            DynamicParameters dbargs = new DynamicParameters();
            dbargs.Add(name: "@userid", value: userid);
           

            var data = (await _sqlConnection.QueryAsync<UserLocationResultDC>("GetAddressbyUserId", transaction: _transaction, param: dbargs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();
        }
    }
}
