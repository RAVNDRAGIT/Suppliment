using BusinessLayer.Product;
using Dapper;
using DataContract.Product;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Product
{
    public class ProductTypeRepository:GenericRepository<ProductType>,IProductTypeRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public ProductTypeRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }

        public async Task<List<ProductTypeDC>> GetProductTypeList()
        {
            var dbArgs = new DynamicParameters();
           
            var data = (await _sqlConnection.QueryAsync<ProductTypeDC>("[dbo].[GetActiveProductType]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();

        }
    }
}
