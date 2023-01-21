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
    public class OrderMasterRepository : GenericRepository<OrderMaster>, IOrderMasterRepository
    {
        public OrderMasterRepository(DbContext dbContext):base(dbContext)
        {

        }
        public async Task<long> SaveOrder(OrderMaster orderMaster,long userid)
        {
            orderMaster.Created_By = userid;
            orderMaster.Created_Date = DateTime.Now;
            orderMaster.Updated_By = userid;
            orderMaster.Updated_Date = DateTime.Now;
            orderMaster.Status = 1;
           
            long res= await _connection.InsertAsync<OrderMaster>(orderMaster);
            return res;
            
        }

       
    }
}
