using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.Order;
using DataContract;
using DataContract.Delivery;
using DataContract.Order;
using DataContract.Product;
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
        public async Task<long> SubmitOrder(CheckOutOrderDC checkOutOrderDC)
        {
           

            long orderid = 0;
            long currentuserid = _jwtMiddleware.GetUserId() ?? 0;
            var cart= await _mongoHelper.OrderCollection().Find(x => x.Created_By == currentuserid && x.Id== checkOutOrderDC.MongoId).FirstOrDefaultAsync();
            if (cart != null)
            {
                
                var orderMaster = Mapper.Map(cart).ToANew<OrderMaster>();
                orderMaster.UserLocationId = checkOutOrderDC.UserLocationId;
                orderMaster.PaymentType = checkOutOrderDC.PaymentType;

                orderid = await _unitOfWork.OrderMasterRepository.SaveOrder(orderMaster, currentuserid);
                
                if (orderid > 0)
                {
                    List<OrderDetail> orderDetail = Mapper.Map(cart.cartDetails).ToANew<List<OrderDetail>>();
                    orderDetail.ForEach(x =>
                    {
                        x.Created_By = currentuserid;
                        x.Created_Date = DateTime.Now;
                        x.Updated_By = currentuserid;
                        x.Updated_Date = DateTime.Now;
                        x.OrderId = orderid;
                        x.IsActive = true;
                        x.IsDelete = false;
                    });
                    
                    bool orderdetailres = await _unitOfWork.OrderDetailRepository.SaveOrderDetail(orderDetail);
                    if (orderdetailres && orderMaster.PaymentType.ToUpper()=="ONLINE")
                    {
                        _unitOfWork.Commit();
                    }

                    else
                    {
                        var orderdetailresult = await _unitOfWork.OrderDetailRepository.GetOrderDetailsbyOrderId(orderid);
                        List<ProductQuantityDC> productQuantities = new List<ProductQuantityDC>();
                        foreach (var orddtl in orderdetailresult)
                        {
                            ProductQuantityDC productQuantityDC = new ProductQuantityDC();
                            productQuantityDC.Quantity = orddtl.Quantity;
                            productQuantityDC.ProductMasterId = orddtl.ProductId;
                            productQuantities.Add(productQuantityDC);
                        }
                        long res = await _unitOfWork.OrderMasterRepository.UpdateOrderStock(productQuantities, currentuserid);
                        if (res >= 0)
                        {

                            var cartres = await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == checkOutOrderDC.MongoId);
                            if (cartres.IsAcknowledged)
                            {

                                _unitOfWork.Commit();
                               
                            }
                        }

                    }
                }
            }
            return orderid;
        }
    }
}
