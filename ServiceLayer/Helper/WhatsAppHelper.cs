using DataContract.Payment;
using DataContract.WhatsApp;
using DataLayer.Context;
using DataLayer.Interface;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WhatsappBusiness.CloudApi;
using WhatsappBusiness.CloudApi.Configurations;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.Requests;
using static MongoDB.Driver.WriteConcern;
using WhatsappBusiness.CloudApi.Webhook;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;

namespace ServiceLayer.Helper
{
    public class WhatsAppHelper
    {
        private readonly DbContext _dbContext;
        private readonly IUnitOfWork _unitofwork;
        public WhatsAppHelper(DbContext dbContext, IUnitOfWork unitofwork)
        {
            _dbContext = dbContext;
            _unitofwork=unitofwork;
        }
        public async Task<bool> SendSMS(string message, string phonenumber)
        {
            using (var client = new HttpClient())
            {
                string token = _dbContext.GetWhatsAppToken();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer" + " " + token);

                string url = _dbContext.GetWhatsAppUrl();

                Language language = new Language()
                {
                    code = "en_US"
                };
                Template template = new Template()
                {
                    name = message,
                    language = language
                };
                MessageRequestDC messageRequestDC = new MessageRequestDC()
                {
                    template = template,
                    to = phonenumber,
                    type = "text",
                    messaging_product = "whatsapp",


                };
                var result = await client.PostAsJsonAsync(url, messageRequestDC);
                if (result != null)
                {


                    string resultContent = await result.Content.ReadAsStringAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> SendMessgae(long orderid)
        {
            var httpClient = new HttpClient();

            httpClient.BaseAddress = WhatsAppBusinessRequestEndpoint.BaseAddress;
            WhatsAppBusinessCloudApiConfig whatsAppConfig = new WhatsAppBusinessCloudApiConfig();
            whatsAppConfig.WhatsAppBusinessPhoneNumberId = _dbContext.GetWhatsAppBusinessPhoneNumberId();
            whatsAppConfig.WhatsAppBusinessAccountId = _dbContext.GetWhatsAppBusinessAccountId();
            whatsAppConfig.WhatsAppBusinessId = _dbContext.GetWhatsAppBusinessId();
            whatsAppConfig.AccessToken = _dbContext.GetWhatsAppToken();
            //create WhatsAppBusiness API client instance
            var whatsAppBusinessClient = new WhatsAppBusinessClient(httpClient, whatsAppConfig);
            var data = await _unitofwork.OrderMasterRepository.GetOrder(orderid);
            if (data != null)
            {
                var user = await _unitofwork.UserLocationRepository.GetById(data.UserLocationId);
                string message = "Dear " + user.Name + " Your Order " + data.Id + " for ₹ " + data.TotalMrp  +" has been confirmed & will reach you shortly 🛳" + " Thanks for shopping with us!";
               

                TextMessageRequest textMessageRequest = new TextMessageRequest();
                textMessageRequest.To ="91"+user.Mobile;
                textMessageRequest.Text = new WhatsAppText();
                textMessageRequest.Text.Body = message;
                textMessageRequest.Text.PreviewUrl = false;

                var results = await whatsAppBusinessClient.SendTextMessageAsync(textMessageRequest);
                if (results.Messages.Count > 0)
                {
                    return true;
                }
            }
            return false;
            
        }
    }
}
