using DataContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;
using ServiceLayer.Helper;
using ServiceLayer.Order;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        public OrderService _orderService;
        public WhatsAppHelper _whatsAppHelper;
        public OrderController(OrderService orderService, WhatsAppHelper whatsAppHelper)
        {

            _orderService = orderService;
            _whatsAppHelper = whatsAppHelper;
        }
        [HttpGet]
        //[Authorize(Roles ="User")]
        public async Task<IActionResult> PlaceOrder(string mongoId)
        {
            
            long? res = await _orderService.SubmitOrder(mongoId);
            if (res!=null && res>0)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("Something went wrong please try again later!!");
            }
        }

        //[HttpGet]
        //public IActionResult SendSMs()
        //{
        //    _whatsAppHelper.SendSMS();
        //    return Ok(true);
        //}
    }
}
