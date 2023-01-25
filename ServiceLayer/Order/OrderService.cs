using AgileObjects.AgileMapper;
using BusinessLayer;
using BusinessLayer.Order;
using DataContract;
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
        public IOrderMasterRepository _orderMasterRepository;
        public IOrderDetailRepository _orderDetailRepository;
        public MongoHelper _mongoHelper;
    
        public OrderService(IUnitOfWork unitOfWork,   JwtMiddleware jwtMiddleware,MongoHelper mongoHelper)
        {
           
            _unitOfWork = unitOfWork;
            _mongoHelper = mongoHelper;
            //var mongoClient = new MongoClient(
            //  _unitOfWork.MongoConString());

            //var mongoDatabase = mongoClient.GetDatabase(
            //    _unitOfWork.MongoDbName());
            //_cart = mongoDatabase.GetCollection<Cart>(
            //    _unitOfWork.MongoOrderCollection());
            _jwtMiddleware = jwtMiddleware;
            userid = _jwtMiddleware.GetUserId();
      
           
        }
        public async Task<long?> SubmitOrder(string mongoId)
        {
           

            long? orderid = null;
            long currentuserid = userid ?? 0;
            var cart= await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid && x.Id== mongoId).FirstOrDefaultAsync();
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
                        List<ProductQuantityDC> productQuantities = new List<ProductQuantityDC>();
                        foreach (var data in orderDetail)
                        {
                            ProductQuantityDC productQuantityDC = new ProductQuantityDC();
                            productQuantityDC.Quantity = data.Quantity;
                            productQuantityDC.ProductMasterId = data.ProductId;
                            productQuantities.Add(productQuantityDC);
                        }
                        long res = await _unitOfWork.OrderMasterRepository.UpdateOrderStock(productQuantities, currentuserid);
                        if (res >= 0 )
                        {

                            var cartres = await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == cart.Id);
                            if (cartres.IsAcknowledged)
                            {
                                orderid = orderresult;
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
