using BusinessLayer.ProductMaster;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Product;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

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


        public async Task<bool> AddProduct(ProductMaster productMaster,long userid)
        {
            productMaster.Created_By = userid;
            productMaster.Created_Date= DateTime.Now;
            productMaster.Updated_By = userid;
            productMaster.Updated_Date = DateTime.Now;
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

        public async Task<List<ProductMaster>> GetProductDynamically(ProductFilterDC productFilterDC)

        {
            var dbArgs = new DynamicParameters();
            if (productFilterDC.productid != null)
            {
                dbArgs.Add(name: "@productid", value: productFilterDC.productid);
            }
            //else
            //{
            //    dbArgs.Add(name: "@productid", value:null);
            //}

            if(productFilterDC.productname != null)
            {
                dbArgs.Add(name: "@productname", value: productFilterDC.productname);

            }
            if (productFilterDC.skip != null)
            {
                dbArgs.Add(name: "@skip", value: productFilterDC.skip);

            }
            if (productFilterDC.take != null)
            {
                dbArgs.Add(name: "@take", value: productFilterDC.take);
            }

            var data = (await _sqlConnection.QueryAsync<ProductMaster>("[dbo].[GetProductDynamically]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();
        }
    }
}
