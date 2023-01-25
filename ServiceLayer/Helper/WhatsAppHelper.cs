using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Twilio;
//using Twilio.Rest.Api.V2010.Account;

namespace ServiceLayer.Helper
{
    public class WhatsAppHelper
    {
        public void SendSMS()
        {
            // Find your Account SID and Auth Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            //string accountSid = Environment.GetEnvironmentVariable("AC5753ce3a684d0d4e23f6d65dd408336f");
            //string authToken = Environment.GetEnvironmentVariable("94371867ea882230cc19b3cdbc8fd71c");

            //TwilioClient.Init("AC5753ce3a684d0d4e23f6d65dd408336f", "94371867ea882230cc19b3cdbc8fd71c");

            //var message = MessageResource.Create(
            //    from: new Twilio.Types.PhoneNumber("whatsapp:+918827617860"),
            //    body: "Hi WhatsApp Testing!",
            //    to: new Twilio.Types.PhoneNumber("whatsapp:+919039629013")
            //);
        }
    }
}
