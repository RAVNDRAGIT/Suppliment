using BusinessLayer.Order;
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
    }
}
