using DataContract.Delivery;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class ShippingRocketHelper
    {
        private readonly DbContext _dbContext;
        public ShippingRocketHelper(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> Authenticate()
        {
            // bool finalresult = false;
            string email = _dbContext.GetDeliveryemail();
            string password = _dbContext.GetDeliverypassword();
            string baseurl = _dbContext.GetDeliveryBaseUrl();

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
                var result = await client.PostAsJsonAsync(baseurl + "/external/auth/login", data);
                if (result != null)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    if (resultContent != null)
                    {
                        var authenticateResponse = JsonSerializer.Deserialize<AuthenticateResponseDC>(resultContent);
                        return authenticateResponse.token;
                    }
                    else
                    {
                        return null;
                    }


                }
                else
                {
                    return null;
                }
            }

        }

        public async Task<string> GetEtd(ServiciabilityDC serviceableRequestDC)
        {
            try
            {
                string etd = null;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer" + " " + serviceableRequestDC.token);



                    string baseurl = _dbContext.GetDeliveryBaseUrl();
                    var json = JsonSerializer.Serialize(serviceableRequestDC);
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(baseurl + "/external/courier/serviceability"),

                        Content = new StringContent(json, Encoding.UTF8, "application/json"),

                    };
                    var response = client.SendAsync(request).ConfigureAwait(false);

                    var responseInfo = response.GetAwaiter().GetResult();
                    if (responseInfo.StatusCode == HttpStatusCode.OK)
                    {
                        string resultContent = await responseInfo.Content.ReadAsStringAsync();

                        if (resultContent != null)
                        {
                            var servicableresponse = JsonSerializer.Deserialize<ServicableResponseDC>(resultContent);
                            if (servicableresponse != null)
                            {
                                int comapntcourierid = servicableresponse.data.recommended_courier_company_id;
                                var couriercompanydata = servicableresponse.data.available_courier_companies.Where(x => x.courier_company_id == servicableresponse.data.recommended_courier_company_id).FirstOrDefault();
                                if (couriercompanydata != null)
                                {
                                    etd = couriercompanydata.etd;
                                }
                            }


                        }
                    }


                }
                return etd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
