using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.Order;
using DataContract;
using DataContract.Order;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Order
{
    public class OrderService 
    {
      //  private readonly IMongoCollection<Cart> _cart;
        public IUnitOfWork _unitOfWork;
        private long? userid;
        public JwtMiddleware _jwtMiddleware;
        public MongoHelper _mongoHelper;
    
        public OrderService(IUnitOfWork unitOfWork,   JwtMiddleware jwtMiddleware,MongoHelper mongoHelper)
        {
           
            _unitOfWork = unitOfWork;
            _mongoHelper = mongoHelper;
            
            _jwtMiddleware = jwtMiddleware;
           
      
           
        }
        public async Task<long?> SubmitOrder(CheckOutOrderDC checkOutOrderDC)
        {
           

            long? orderid = null;
            long currentuserid = _jwtMiddleware.GetUserId() ?? 0;
            var cart= await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid && x.Id== checkOutOrderDC.MongoId).FirstOrDefaultAsync();
            if (cart != null)
            {
                
                var orderMaster = Mapper.Map(cart).ToANew<OrderMaster>();
                long orderresult = await _unitOfWork.OrderMasterRepository.SaveOrder(orderMaster, currentuserid);
                
                if (orderresult > 0)
                {
                    List<OrderDetail> orderDetail = Mapper.Map(cart.cartDetails).ToANew<List<OrderDetail>>();
                    orderDetail.ForEach(x =>
                    {
                        x.Created_By = currentuserid;
                        x.Created_Date = DateTime.Now;
                        x.Updated_By = currentuserid;
                        x.Updated_Date = DateTime.Now;
                        x.OrderId = orderresult;
                        x.IsActive = true;
                        x.IsDelete = false;
                    });
                    
                    bool orderdetailres = await _unitOfWork.OrderDetailRepository.SaveOrderDetail(orderDetail);
                    if (orderdetailres)
                    {
                        _unitOfWork.Commit();
                       


                    }
                }
            }
            return orderid;
        }
    }
}
