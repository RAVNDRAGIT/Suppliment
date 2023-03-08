using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Helper;

namespace Suppliment.API.Controllers.Notification
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationHelper _notificationHelper;
        public NotificationController(NotificationHelper notificationHelper)
        {
            _notificationHelper = notificationHelper;
            
        }

        [HttpGet]
        public async Task SendNotification()
        {
            NotificationDetailDC notification = new NotificationDetailDC()
            {
                title = "Order #43",
                body = "Theres a new pickup order in line!",
                sound = "default"
            };
            NotificationDC notificationDC = new NotificationDC()
            {
                to = "",
                notification = notification


            };

            await _notificationHelper.SendNotificationAsync(notificationDC);
        }
    }
}
