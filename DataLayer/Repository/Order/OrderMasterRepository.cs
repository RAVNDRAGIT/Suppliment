using BusinessLayer.Order;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Order
{
    public class OrderMasterRepository : GenericRepository<OrderMaster>, IOrderMasterRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;

        public OrderMasterRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<long> SaveOrder(OrderMaster orderMaster,long userid)
        {
            orderMaster.Created_By = userid;
            orderMaster.Created_Date = DateTime.Now;
            orderMaster.Updated_By = userid;
            orderMaster.Updated_Date = DateTime.Now;
            orderMaster.Status = 1;
            orderMaster.IsActive=true;
            orderMaster.IsDelete = false;


            long res= await _sqlConnection.InsertAsync<OrderMaster>(orderMaster, _transaction);
            return res;
            
        }

       

        public async Task<long> UpdateOrderStock(List<ProductQuantityDC> productQuantities,long userid)
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
            dbArgs.Add(name: "@UpdatedBy", value: userid);
            dbArgs.Add(name: "@PRODUCTQUANTITY", value: dt.AsTableValuedParameter("[dbo].[productQuantity]"));
            var res = await _sqlConnection.QueryAsync<long>("[dbo].[UpdateStockQuantity]",param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure);
           

            return res.FirstOrDefault();

        }
    }
}
