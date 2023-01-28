using BusinessLayer.Order;
using DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IOrderDetailRepository
    {
        Task<bool> SaveOrderDetail(List<OrderDetail> orderDetail);
        Task<List<OrderDetailDC>> GetOrderDetailsbyOrderId(long orderid);
    }
}
