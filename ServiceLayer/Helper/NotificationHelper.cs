using DataContract;
using DataContract.Delivery;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helper
{
    public class NotificationHelper
    {
        private DbContext _dbContext;
        public NotificationHelper(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SendNotificationAsync(NotificationDC notificationDC)
        {
            string url = _dbContext.GetFCMUrl();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", notificationDC.to);

               
                var result = await client.PostAsJsonAsync(url, notificationDC);
                if (result.IsSuccessStatusCode )
                {
                    string resultContent = await result.Content.ReadAsStringAsync();
                    //if (resultContent != null)
                    //{
                    //    var authenticateResponse = JsonSerializer.Deserialize<AuthenticateResponseDC>(resultContent);
                    //    return authenticateResponse.token;
                    //}
                    //else
                    //{
                    //    return null;
                    //}


                }
                else
                {
                    //return null;
                }
            }
        }
    }
}
