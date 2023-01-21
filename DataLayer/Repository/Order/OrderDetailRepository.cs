using BusinessLayer.Order;
using Dapper.Contrib.Extensions;
using DataContract;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Order
{
    public class OrderDetailRepository : GenericRepository<OrderDetailRepository>, IOrderDetailRepository
    {
        public OrderDetailRepository(DbContext dbContext) : base(dbContext) { }
        public async Task<bool> SaveOrderDetail(List<OrderDetail> orderDetail)
        {
            

            long res= await _connection.InsertAsync<List<OrderDetail>>(orderDetail);
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
