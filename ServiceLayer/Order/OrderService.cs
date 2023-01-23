using AgileObjects.AgileMapper;
using BusinessLayer.Order;
using DataContract;
using DataLayer.Interface;
using ServiceLayer.Interface.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Order
{
    public class OrderService : IOrderService
    {
        public IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> SubmitOrder(List<OrderDetailDC> orderDetailDC, long? userid)
        {
            bool result = false;
            long currentuserid = userid ?? 0;
            orderDetailDC.ForEach(x =>
            {
                x.TotalMrp = x.Mrp * x.Quantity;
                x.TotalPrice = x.Price * x.Quantity;
                x.TotalDiscount = (x.Mrp * x.Quantity) - (x.Price * x.Quantity);

            });
            double TotalMrp = orderDetailDC.Sum(x => x.TotalMrp);
            double TotalPrice = orderDetailDC.Sum(x => x.TotalPrice);
            double TotalDiscount = orderDetailDC.Sum(x => x.TotalDiscount);

            OrderMaster orderMaster = new OrderMaster
            {
                IsActive = true,
                IsDelete = false,
                IsPaid = false,
                TotalDiscount = TotalDiscount,
                TotalMrp = TotalMrp,
                TotalPrice = TotalPrice
            };

            long orderresult = await _unitOfWork.OrderMasterRepository.SaveOrder(orderMaster, currentuserid);
            if (orderresult > 0)
            {
                List<OrderDetail> orderDetail = Mapper.Map(orderDetailDC).ToANew<List<OrderDetail>>();
                orderDetail.ForEach(x =>
                {
                    x.Created_By = currentuserid;
                    x.Created_Date = DateTime.Now;
                    x.Updated_By = currentuserid;
                    x.Updated_Date = DateTime.Now;
                });

                bool orderdetailres = await _unitOfWork.OrderDetailRepository.SaveOrderDetail(orderDetail);
                if (orderdetailres)
                {
                    _unitOfWork.Commit();
                    result = true;
                }
            }
            return result;
        }
    }
}
