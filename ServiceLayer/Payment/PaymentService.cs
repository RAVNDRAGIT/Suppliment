
using BusinessLayer.Order;
using DataContract;
using DataContract.Payment;
using DataLayer.Context;
using DataLayer.Infrastructure;
using DataLayer.Interface;
using DataLayer.Repository.Auth;
using DataLayer.Repository.Order;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Payment
{
    public class PaymentService
    {
        private readonly MongoHelper _mongoHelper;

       
        public DbContext _dbContext;
        private readonly string Baseurl;
        private readonly string notifyurl;
        private readonly string returnurl;
        private readonly string clientid;
        private readonly string clientsecret;
        private readonly string apiversion;
        public IUnitOfWork _unitOfWork;
        private readonly long userid;
        private JwtMiddleware _jwtMiddleware;
        public PaymentService(IUnitOfWork unitOfWork, MongoHelper mongoHelper,  DbContext dbContext, JwtMiddleware jwtMiddleware)
        {
            _unitOfWork = unitOfWork;
            _mongoHelper = mongoHelper;
            _dbContext = dbContext;
             Baseurl = _dbContext.GetBaseUrl();
             notifyurl = _dbContext.GetNotifyUrl();
             returnurl = _dbContext.GetReturnUrl();
            clientid= _dbContext.GetClientId();
            clientsecret = _dbContext.GetClientSecret();
            apiversion = _dbContext.GetApiVersion();
            _jwtMiddleware = jwtMiddleware;
        }

        public async Task<string> CreateOrderAsync(long orderid)
        {
             
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://sandbox.cashfree.com/pg/orders");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-client-id", clientid);
                client.DefaultRequestHeaders.Add("x-client-secret", clientsecret);
                client.DefaultRequestHeaders.Add("x-api-version", apiversion);

               
                var orderdata = await _unitOfWork.OrderMasterRepository.GetOrder(orderid);
                double orderAmount = 0;
                if (orderdata != null)
                {
                    orderAmount = orderdata.TotalPrice;
                }
                var customerdata = await _unitOfWork.UserRepository.GetUser(orderdata.Created_By);
                string customeremailid = null;
                string customerid = null;
                string customerphone = null;
                if (customerdata != null)
                {
                     customeremailid = customerdata.Email;
                     customerid = Convert.ToString(customerdata.Id);
                     customerphone = customerdata.Mobile;
                }

                OrderMeta orderMeta = new OrderMeta
                {
                    notify_url = notifyurl,
                    return_url = returnurl
                };

                CustomerDetails customerDetails = new CustomerDetails
                {
                    customer_email = customeremailid,
                    customer_id = customerid,
                    customer_phone = customerphone
                };

                CreateOrderRequestDC createOrderRequestDC = new CreateOrderRequestDC
                {
                    order_id =  Convert.ToString(orderid),
                    order_amount = orderAmount,
                    order_currency = "INR",
                    order_expiry_time = DateTime.Now.AddMinutes(18),
                    customer_details = customerDetails,
                    order_meta = orderMeta

                };


                var result = await client.PostAsJsonAsync(Baseurl, createOrderRequestDC);
                if(result!=null)
                {

                
                string resultContent = await result.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<CreateOrderResponseDC>(resultContent);
                    res.IsActive = true;
                    res.IsDelete = false;
                    res.Created_By = userid;
                    res.Created_Date= DateTime.Now;
                    res.Updated_By=userid;
                    res.Updated_By= userid;
                await _mongoHelper.OrderResponseCollection().InsertOneAsync(res);
                return res.payment_session_id;
                }
                else
                {
                    return null;
                }
                //return resultContent;
            }
        }

        public async Task<string> CapturePayment(string orderid)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://sandbox.cashfree.com/pg/orders");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-client-id", clientid);
                client.DefaultRequestHeaders.Add("x-client-secret", clientsecret);
                client.DefaultRequestHeaders.Add("x-api-version", apiversion);

                CapturePaymentDC capturePaymentDC = new CapturePaymentDC
                {
                    action = "CAPTURE",
                    amount = 500
                };


                var result = await client.PostAsJsonAsync(Baseurl + orderid + "/authorization", capturePaymentDC);

                string resultContent = await result.Content.ReadAsStringAsync();

                return resultContent;
            }
        }

        public async Task<bool> AfterPayment(long orderid)
        {
            bool finalresult = false;
            string strorderid = Convert.ToString(orderid);
            long userid = _jwtMiddleware.GetUserId()??0;


            using (var client = new HttpClient())
            {
               
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-client-id", clientid);
                client.DefaultRequestHeaders.Add("x-client-secret", clientsecret);
                client.DefaultRequestHeaders.Add("x-api-version", apiversion);
                var result = await client.GetAsync(Baseurl+"/"+ strorderid);
                string resultContent = await result.Content.ReadAsStringAsync();
                CreateOrderResponseDC createOrderResponseDC = JsonSerializer.Deserialize<CreateOrderResponseDC>(resultContent);
                var data = await _mongoHelper.OrderResponseCollection().Find(x => x.order_id == createOrderResponseDC.order_id).FirstOrDefaultAsync();
                if (data != null)
                {

                    createOrderResponseDC.Updated_By = userid;
                    createOrderResponseDC.Updated_Date = DateTime.Now;
                    createOrderResponseDC.Id = data.Id;
                    createOrderResponseDC.IsActive = true;
                    createOrderResponseDC.IsDelete = false;
                    await _mongoHelper.OrderResponseCollection().ReplaceOneAsync(x => x.Id == data.Id, createOrderResponseDC, new UpdateOptions { IsUpsert = true });
                    bool orderstatus = false;
                    if (createOrderResponseDC.order_status == "PAID")
                    {
                        orderstatus = true;
                    }
                    bool updateorderresult = await _unitOfWork.OrderMasterRepository.UpdateOrderPayment(Convert.ToInt64(data.order_id), orderstatus, userid);
                    if (updateorderresult)
                    {
                        var orderdetailresult = await _unitOfWork.OrderDetailRepository.GetOrderDetailsbyOrderId(Convert.ToInt64(data.order_id));
                        List<ProductQuantityDC> productQuantities = new List<ProductQuantityDC>();
                        foreach (var orddtl in orderdetailresult)
                        {
                            ProductQuantityDC productQuantityDC = new ProductQuantityDC();
                            productQuantityDC.Quantity = orddtl.Quantity;
                            productQuantityDC.ProductMasterId = orddtl.ProductId;
                            productQuantities.Add(productQuantityDC);
                        }
                        long res = await _unitOfWork.OrderMasterRepository.UpdateOrderStock(productQuantities, userid);
                        if (res >= 0)
                        {

                            var cartres = await _mongoHelper.OrderCollection().DeleteOneAsync(x => x.Id == data.Id);
                            if (cartres.IsAcknowledged)
                            {

                                _unitOfWork.Commit();
                                finalresult = true;
                            }
                        }
                       

                    }

                }
            }

              
           
            return finalresult;
            
        }
        //public CreateOrderResponse GetOrderResponse(string Id)
        //{
        //    var res =  _mongoHelper.OrderResponseCollection().Find(x => x.Id == Id).FirstOrDefault();
        //    return res;
        //}
    }
}
