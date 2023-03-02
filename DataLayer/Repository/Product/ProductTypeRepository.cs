using BusinessLayer.Product;
using Dapper;
using Dapper.Contrib.Extensions;
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

        public async Task<long> AddProductType(ProductType productType)
        {
            productType.IsActive = true;
            productType.IsDelete = false;
            productType.Created_Date=DateTime.Now;
            productType.Created_By = 0;
            productType.Updated_Date = DateTime.Now;
            productType.Updated_By = 0;

            var res = await _sqlConnection.InsertAsync(productType,_transaction);
            return res;
        }

        public async Task<List<ProductTypeDC>> GetProductTypeList()
        {
            var dbArgs = new DynamicParameters();
           
            var data = (await _sqlConnection.QueryAsync<ProductTypeDC>("[dbo].[GetActiveProductType]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();

        }

        public async Task<long> Save(List<ProductType> list)
        {
            var data = await _sqlConnection.InsertAsync<List<ProductType>>(list, _transaction);
            return data;
        }
    }
}
