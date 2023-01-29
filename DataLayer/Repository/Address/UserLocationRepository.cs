using BusinessLayer;
using Dapper.Contrib.Extensions;
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

            var data = await _sqlConnection.InsertAsync<UserLocation>(userLocations, _transaction);
                return data;
        }

        public async Task<UserLocation> GetById(long id)
        {
           var data = await _sqlConnection.GetAsync<UserLocation>(id,_transaction);
            return data;
        }
    }
}
