using BusinessLayer.ProductMaster;
using Dapper;
using Dapper.Contrib.Extensions;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using System.Data;

namespace DataLayer.Repository.Product
{
    public class ProductMasterRepository : GenericRepository<ProductMaster>,IProductMasterRepository
    {
        //private IDbTransaction _transaction;
        public ProductMasterRepository(DbContext dbContext) : base(dbContext)
        {
            //_transaction= transaction;
        }


        public async Task<bool> AddProduct(ProductMaster productMaster)
        {
            //productMaster.Created_By = "RAVINDRA";
            //productMaster.Created_Date= DateTime.Now;
            //productMaster.Updated_By = "RAVINDRA";
            //productMaster.Updated_Date = DateTime.Now;
            int res = await _connection.InsertAsync<ProductMaster>(productMaster);
            if(res>0)
            {
                
                return true;
            }
            return false;
        }

        
    }
}
