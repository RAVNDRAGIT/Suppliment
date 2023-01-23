using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;
using ServiceLayer.Helper;
using ServiceLayer.Interface.IHelper;
using ServiceLayer.Interface.IService;
using ServiceLayer.Order;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IOrderService _orderService;
        public IAuthService _authService;
        public IWhatsApp _whatsAppHelper;
        public OrderController(IOrderService orderService, IAuthService authService, IWhatsApp whatsAppHelper)
        {

            _orderService = orderService;
            _authService = authService;
            _whatsAppHelper = whatsAppHelper;
        }
        [HttpPost]

        public async Task<IActionResult> PlaceOrder(List<OrderDetailDC> orderdetailDc)
        {
            long? userid = _authService.GetUserId();
            bool res = await _orderService.SubmitOrder(orderdetailDc, userid);
            if (res)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Something went wrong please try again later!!");
            }
        }

        [HttpGet]
        public IActionResult SendSMs()
        {
            _whatsAppHelper.SendSMS();
            return Ok(true);
        }
    }
}
