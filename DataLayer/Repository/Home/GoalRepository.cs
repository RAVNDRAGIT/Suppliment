using BusinessLayer.Home;
using Dapper;
using Dapper.Contrib.Extensions;
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
    public class GoalRepository : GenericRepository<Goal>, IGoalRepository
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

        public async Task<long> Save(List<Goal> goals)
        {
            long res = await _sqlConnection.InsertAsync<List<Goal>>(goals, _transaction);
            return res;
        }

        public async Task<long> AddGoal(Goal  goal)
        {
          
            goal.IsActive = true;
            goal.IsDelete = false;
            goal.Created_Date = DateTime.Now;
            goal.Created_By = 0;
            goal.Updated_By = 0;
            goal.Updated_Date = DateTime.Now;
            long res = await _sqlConnection.InsertAsync<Goal>(goal, _transaction);
            return res;
        }
    }
}