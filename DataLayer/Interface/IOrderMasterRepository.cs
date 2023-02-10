using BusinessLayer.Order;
using DataContract.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IOrderMasterRepository
    {
        Task<long> SaveOrder(OrderMaster orderMaster,long userid);
        Task<long> UpdateOrderStock(List<ProductQuantityDC> productQuantities, long userid);
        Task<OrderMaster> GetOrder(long orderid);
        Task<bool> UpdateOrderPayment(long orderid,bool ispaid,long userid);
        Task<long> UpdateDeliveryToken(long orderid, long userid, string token);
    }
}
