using BusinessLayer.Order;
using Dapper.Contrib.Extensions;
using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Order
{
    public class OrderDetailRepository : GenericRepository<OrderDetailRepository>, IOrderDetailRepository
    {
        private SqlConnection _sqlConnection;

        private IDbTransaction _dbTransaction;
      
        public OrderDetailRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction):base( sqlConnection,  dbTransaction)
        {
            _dbTransaction = dbTransaction;
            _sqlConnection = sqlConnection;
        }
        public async Task<bool> SaveOrderDetail(List<OrderDetail> orderDetail)
        {
            

            long res= await _sqlConnection.InsertAsync<List<OrderDetail>>(orderDetail, _transaction);
            if(res>0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
