using DataContract.Delivery;
using DataContract.Payment;
using DataContract.Product;
using DataLayer.Context;
using DataLayer.Interface;
using DnsClient;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MongoDB.Driver.WriteConcern;

namespace ServiceLayer.Delivery
{
    public class DeliveryService
    {
  
        private readonly IUnitOfWork _unitofWork;
        private readonly JwtMiddleware _jwtMiddleware;
        private readonly MongoHelper _mongoHelper;
        private readonly ShippingRocketHelper _shippingRocketHelper;
        public DeliveryService(ShippingRocketHelper shippingRocketHelper, IUnitOfWork unitOfWork, JwtMiddleware jwtMiddleware, MongoHelper mongoHelper)
        {

           
            _unitofWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
            _mongoHelper = mongoHelper;
            _shippingRocketHelper = shippingRocketHelper;
        }


        public async Task<string> Authenticate()
        {
          string res= await  _shippingRocketHelper.Authenticate();
            return res;

        }

        public async Task<string> GetServicable(ServiceableRequestDC serviceableRequestDC)
        {
            string etd = null;
            long? userid = _jwtMiddleware.GetUserId();
            bool result = false;
            int weight = 0;
            
            var cart = await _mongoHelper.OrderCollection().Find(x => x.Created_By == userid && x.Id == serviceableRequestDC.mongoId).FirstOrDefaultAsync();
            List<ProductQuantityDC> list = new List<ProductQuantityDC>();
            if (cart.cartDetails != null)
            {
                cart.cartDetails.ForEach(x =>
                {
                    ProductQuantityDC productQuantityDC = new ProductQuantityDC();
                    productQuantityDC.Quantity = x.Quantity;
                    productQuantityDC.ProductMasterId = x.ProductId;
                    list.Add(productQuantityDC);
                });


                weight = await _unitofWork.ProductMasterRepository.GetWeight(list);

                var servicablerequestdata = new ServiciabilityDC
                {
                    pickup_postcode = 462026,
                    delivery_postcode = serviceableRequestDC.delivery_postcode,
                    length = 0,
                    height = 0,
                    breadth = 0,
                    weight = weight,
                    cod = true,

                    //declared_value= Convert.ToInt32( productdata.TotalPrice),
                    mode = "SURFACE",
                    only_local = 0,
                    token= serviceableRequestDC.token


                };

                 etd = await _shippingRocketHelper.GetEtd(servicablerequestdata);
               
            }
            return etd;
        }

    }
}
