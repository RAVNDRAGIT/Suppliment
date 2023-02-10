using BusinessLayer.Home;
using Dapper;
using DataContract.Home;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Order;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Home
{
    public class GoalRepository : GenericRepository<GoalRepository>, IGoalRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public GoalRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<List<GoalDC>> GetActiveGoalsAsync()
        {
            var dbArgs = new DynamicParameters();


            //var data =(_sqlConnection.QueryMultiple<AddressDetailDC>)
            var data = (await _sqlConnection.QueryAsync <GoalDC>("GetActiveGoals", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}