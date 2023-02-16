using BusinessLayer.Product;
using BusinessLayer.Users;
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

        public async Task<List<DynamicProductDC>> GetLikeProduct(long producttypeid, long productid)
        {
            var dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@producttypeid", value: producttypeid);
            dbArgs.Add(name: "@productid", value: productid);
            var data = (await _sqlConnection.QueryAsync<DynamicProductDC>("[dbo].[GetLikeProduct]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();
        }

        public async Task<ProductMaster> GetProduct(long id)
        {
            var data = await _sqlConnection.GetAsync<ProductMaster>(id,_transaction);
            return data;
        }

        public async Task<List<DynamicProductDC>> GetProductDynamically(ProductFilterDC productFilterDC)

        {
            var dbArgs = new DynamicParameters();
            if (productFilterDC.productid != null)
            {
                dbArgs.Add(name: "@productid", value: productFilterDC.productid);
            }
            

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

            if (productFilterDC.categoryid != null)
            {
                dbArgs.Add(name: "@categoryid", value: productFilterDC.categoryid);
            }

            if (productFilterDC.producttypeid != null)
            {
                dbArgs.Add(name: "@producttypeid", value: productFilterDC.producttypeid);
            }

            if (productFilterDC.goalid != null)
            {
                dbArgs.Add(name: "@goalid", value: productFilterDC.goalid);
            }


            var data = (await _sqlConnection.QueryAsync<DynamicProductDC>("[dbo].[GetProductDynamically]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();
        }

        public async Task<int> GetWeight(List<ProductQuantityDC> productQuantities)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductMasterId");
            dt.Columns.Add("Quantity");
            foreach (var data in productQuantities)
            {
                var dr = dt.NewRow();
                dr["ProductMasterId"] = data.ProductMasterId;
                dr["Quantity"] = data.Quantity;
                dt.Rows.Add(dr);
            }
          
            dbArgs.Add(name: "@PRODUCTQUANTITY", value: dt.AsTableValuedParameter("[dbo].[productQuantity]"));
            var res = await _sqlConnection.QueryAsync<int>("[dbo].[GetProductWeight]", param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure);


            return res.FirstOrDefault();
        }
    }
}
