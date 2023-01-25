using BusinessLayer.ProductMaster;
using Dapper;
using Dapper.Contrib.Extensions;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataLayer.Repository.Product
{
    public class ProductMasterRepository : GenericRepository<ProductMaster>,IProductMasterRepository
    {

        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public ProductMasterRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }


        public async Task<bool> AddProduct(ProductMaster productMaster)
        {
            //productMaster.Created_By = "RAVINDRA";
            //productMaster.Created_Date= DateTime.Now;
            //productMaster.Updated_By = "RAVINDRA";
            //productMaster.Updated_Date = DateTime.Now;
            int res = await _sqlConnection.InsertAsync<ProductMaster>(productMaster,_transaction);
            if(res>0)
            {
                
                return true;
            }
            return false;
        }

        public async Task<ProductMaster> GetProduct(long id)
        {
            var data = await _sqlConnection.GetAsync<ProductMaster>(id,_transaction);
            return data;
        }
    }
}
