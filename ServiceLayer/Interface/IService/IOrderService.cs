using DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface.IService
{
    public interface IOrderService
    {
        Task<bool> SubmitOrder(List<OrderDetailDC> orderDetailDC,long userid);
    }
}
