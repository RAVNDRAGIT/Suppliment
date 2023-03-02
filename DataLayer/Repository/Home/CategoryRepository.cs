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
    public class CategoryRepository : GenericRepository<CategoryRepository>, ICategoryRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public CategoryRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<List<CategoryDC>> GetAllActiveCategories()
        {
            var dbArgs = new DynamicParameters();


            //var data =(_sqlConnection.QueryMultiple<AddressDetailDC>)
            var data = (await _sqlConnection.QueryAsync<CategoryDC>("GETACTIVECATEGORIES", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            if (data != null)
            {
                return data.ToList();
            }
            else
            {
                return null;
            }
        }

        public async Task<long> Save(List<CategoryMaster> list)
        {
            var data = await _sqlConnection.InsertAsync<List<CategoryMaster>>(list, _transaction);
            return data;
        }

        public async Task<long> Save(CategoryMaster category)
        {
            category.IsActive = true;
            category.IsDelete = false;
            category.Created_By = 0;
            category.Created_Date = DateTime.Now;
            category.Updated_By = 0;
            category.Updated_Date = DateTime.Now;
            var data = await _sqlConnection.InsertAsync<CategoryMaster>(category, _transaction);
            return data;
        }
    }
}
