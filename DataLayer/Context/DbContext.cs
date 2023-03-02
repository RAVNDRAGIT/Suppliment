using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;


        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;


        }

        public string AuthKey()
        {
            string _authKey = _configuration.GetSection("Key").GetSection("userKey").Value;
            return _authKey;
        }

        public string MongoConString()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("ConnectionString").Value;
            return con;
        }

        public string MongoDbName()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("DatabaseName").Value;
            return con;
        }

        public string MongoOrderCollection()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("OrderCollectionName").Value;
            return con;
        }

        public string MongoOrderPaymentCollection()
        {
            string con = _configuration.GetSection("MongoDbDC").GetSection("OrderPayment").Value;
            return con;
        }

        public string GetNotifyUrl()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("notify_url").Value;
            return url;
        }

        public string GetReturnUrl()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("return_url").Value;
            return url;
        }

        public string GetBaseUrl()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("base_url").Value;
            return url;
        }
        public string GetClientId()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("client-id").Value;
            return url;
        }

        public string GetClientSecret()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("client-secret").Value;
            return url;
        }
        public string GetApiVersion()
        {
            string url = _configuration.GetSection("PaymentIntegration").GetSection("api-version").Value;
            return url;
        }

        public string GetDeliveryBaseUrl()
        {
            string url = _configuration.GetSection("DeliveryIntegration").GetSection("base_url").Value;
            return url;
        }

        public string GetDeliveryemail()
        {
            string url = _configuration.GetSection("DeliveryIntegration").GetSection("email").Value;
            return url;
        }

        public string GetDeliverypassword()
        {
            string url = _configuration.GetSection("DeliveryIntegration").GetSection("password").Value;
            return url;
        }

        public string GetWhatsAppToken()
        {
            string token = _configuration.GetSection("WhatsappIntegration").GetSection("token").Value;
            return token;
        }
        public string GetWhatsAppUrl()
        {
            string url = _configuration.GetSection("WhatsappIntegration").GetSection("url").Value;
            return url;
        }
        public string GetWhatsAppBusinessAccountId()
        {
            string businessaccid = _configuration.GetSection("WhatsappIntegration").GetSection("WhatsAppBusinessAccID").Value;
            return businessaccid;
        }

        public string GetWhatsAppBusinessPhoneNumberId()
        {
            string businesphonenumberid = _configuration.GetSection("WhatsappIntegration").GetSection("WhatsAppBusinessPhoneNumberId").Value;
            return businesphonenumberid;
        }

        public string GetWhatsAppBusinessId()
        {
            string businessid = _configuration.GetSection("WhatsappIntegration").GetSection("WhatsAppBusinessId").Value;
            return businessid;
        }
        public string GetGoogleClientId()
        {
            string clientid = _configuration.GetSection("GoogleIntegration").GetSection("clientid").Value;
            return clientid;
        }

        public string GetSqlConnection()
        {
            string conn = _configuration.GetSection("ConnectionStrings").GetSection("SqlConnection").Value;
            return conn;
        }

        public string GetCloudinaryName()
        {
            string clname = _configuration.GetSection("Cloudinary").GetSection("CloudName").Value;
            return clname;
        }

        public string GetCloudinaryApiKey()
        {
            string clname = _configuration.GetSection("Cloudinary").GetSection("ApiKey").Value;
            return clname;
        }

        public string GetCloudinaryApiSecret()
        {
            string clname = _configuration.GetSection("Cloudinary").GetSection("ApiSecret").Value;
            return clname;
        }

        public string GetCloudinaryUrl()
        {
            string clurl = _configuration.GetSection("Cloudinary").GetSection("Url").Value;
            return clurl;
        }
        public IDbConnection CreateConnection()
        {
            //_connection = new SqlConnection();

            string constr = GetSqlConnection();
            //_connection.ConnectionString = constr;
            return new SqlConnection(constr);
            //return _dbContext.CreateConnection();

        }

    }
}

