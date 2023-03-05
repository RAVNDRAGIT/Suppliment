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
    public class ProductImageRepository : GenericRepository<ProductImage>, IProductImageRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public ProductImageRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<long> SaveImage(List<ProductImage> images)
        {
            var res = await _sqlConnection.InsertAsync<List<ProductImage>>(images, _transaction);
            return res;
        }
        public async Task<List<ProductImageDC>> GetProductImagebyProductId(long productid)
        {
            var dbArgs = new DynamicParameters();

            dbArgs.Add(name: "@ProductId", value: productid);



            var data = (await _sqlConnection.QueryAsync<ProductImageDC>("[dbo].[GetProductImagesbyProductId]", transaction: _transaction, param: dbArgs, commandType: CommandType.StoredProcedure, commandTimeout: 30000));
            return data.ToList();
        }
    }
}
