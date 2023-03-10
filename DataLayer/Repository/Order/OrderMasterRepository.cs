using BusinessLayer.Order;
using BusinessLayer.Users;
using Dapper;
using Dapper.Contrib.Extensions;
using DataContract.Product;
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

       public async Task<long> UpdateDeliveryToken(long orderid,long userid,string token)
        {
            DynamicParameters dbArgs = new DynamicParameters();
            dbArgs.Add(name: "@token", value: token);
            dbArgs.Add(name: "@updateby", value: userid);
            dbArgs.Add(name: "@orderid", value: orderid);
            var res = await _sqlConnection.ExecuteAsync("[dbo].[UpdateDeliverytoken]", param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure);
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
            var res = await _sqlConnection.ExecuteAsync("[dbo].[UpdateStockQuantity]",param: dbArgs, transaction: _transaction, commandType: CommandType.StoredProcedure);
           

            return res;

        }

        public async Task<OrderMaster> GetOrder(long orderid)
        {
            var data = await _sqlConnection.GetAsync<OrderMaster>(orderid, _transaction);
            return data;
        }

        public async Task<bool> UpdateOrderPayment(long orderid,bool ispaid,long userid)
        {
            var data = await _sqlConnection.GetAsync<OrderMaster>(orderid, _transaction);
            if(data!=null)
            {
                data.IsPaid = ispaid;
                data.Updated_Date = DateTime.Now;
                data.Updated_By = userid;
            }

            bool result = await _sqlConnection.UpdateAsync<OrderMaster>(data, _transaction);
            return result;
        }

        public async Task<bool> UpdateSentSms(long orderid,bool issentsms,long userid)
        {
            var data = await _sqlConnection.GetAsync<OrderMaster>(orderid, _transaction);
            if (data != null)
            {
                data.IsMsgSent = issentsms;
                data.Updated_Date = DateTime.Now;
                data.Updated_By = userid;
            }

            bool result = await _sqlConnection.UpdateAsync<OrderMaster>(data, _transaction);
            return result;
        }
    }
}
