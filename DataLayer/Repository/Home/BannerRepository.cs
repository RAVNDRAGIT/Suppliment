using BusinessLayer.Home;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Home;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Home
{
    public class BannerRepository:GenericRepository<Banner>, IBannerRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public BannerRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<List<BannerDC>> GetBanner()
        {
            var dbArgs = new DynamicParameters();
           
            var data = (await _sqlConnection.QueryAsync<BannerDC>("GetActiveBanners", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<long> SaveBulkBanner(List<Banner> list)
        {
            var res = await _sqlConnection.InsertAsync<List<Banner>>(list, _transaction);
            return res;
        }
    }
}
