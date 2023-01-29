using DataContract.Delivery;
using DataContract.Payment;
using DataLayer.Context;
using DataLayer.Interface;
using DnsClient;
using MongoDB.Bson.IO;
using ServiceLayer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly DbContext _dbContext;
        private readonly IUnitOfWork _unitofWork;
        private readonly JwtMiddleware _jwtMiddleware;
        public DeliveryService(DbContext dbContext, IUnitOfWork unitOfWork, JwtMiddleware jwtMiddleware)
        {

            _dbContext = dbContext;
            _unitofWork = unitOfWork;
            _jwtMiddleware = jwtMiddleware;
        }


        public async Task<bool> Authenticate(long orderid)
        {
            bool finalresult = false;
            string email = _dbContext.GetDeliveryemail();
            string password = _dbContext.GetDeliverypassword();
            string baseurl = _dbContext.GetDeliveryBaseUrl();
            long userid = _jwtMiddleware.GetUserId() ?? 0;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new AuthenticateDC()
                {
                    email = email,
                    password = password
                };
                var result = await client.PostAsJsonAsync(baseurl+ "/external/auth/login", data);
                if (result != null)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    var authenticateResponse = JsonSerializer.Deserialize<AuthenticateResponseDC>(resultContent);
                    var updatetokenresult = await _unitofWork.OrderMasterRepository.UpdateDeliveryToken(orderid, userid, authenticateResponse.token);
                    if (updatetokenresult > 0)
                    {
                        _unitofWork.Commit();
                        finalresult = true;

                    }
                }
            }
            return finalresult;
        }

        public async Task<bool> GetServicable(long orderid)
        {
            bool result = false;
            var productdata = await _unitofWork.OrderMasterRepository.GetOrder(orderid);
            if (productdata != null)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer"+" "+productdata.DeliveryToken);
                    //client.DefaultRequestHeaders.Add("pickup_postcode" , 462026);
                    //client.DefaultRequestHeaders.Add("delivery_postcode" + 462043);
                    //client.DefaultRequestHeaders.Add("pickup_postcode" + 462026);
                    //client.DefaultRequestHeaders.Add("pickup_postcode" + 462026);
                    //client.DefaultRequestHeaders.Add("pickup_postcode" + 462026);
                    //client.DefaultRequestHeaders.Add("pickup_postcode" + 462026);
                    var servicablerequestdata = new ServiciabilityDC
                    {
                        pickup_postcode = 462026,
                        delivery_postcode = 452007,
                        length=2,
                        height=2,
                        breadth=2,
                        weight= 2,
                        cod=false,
                        declared_value= Convert.ToInt32( productdata.TotalPrice),
                        mode= "SURFACE",
                        only_local=1


                    };
                   
                    string baseurl = _dbContext.GetDeliveryBaseUrl();
                    var json = JsonSerializer.Serialize(servicablerequestdata);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(baseurl + "/external/courier/serviceability"),

                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                        
                    };
                    var response = client.SendAsync(request).ConfigureAwait(false);

                    var responseInfo = response.GetAwaiter().GetResult();
                   // var responseresult = await client.GetAsync(baseurl + "/external/courier/serviceability", servicablerequestdata);
                   string resultContent = await responseInfo.Content.ReadAsStringAsync();
                   
                    
                    //if (resultContent)
                    //{
                    //    result = true;
                    //}
                }
            }
            return result;
        }

        //public async Task<long> CreateOrder()
        //{

        //}

    }
}
