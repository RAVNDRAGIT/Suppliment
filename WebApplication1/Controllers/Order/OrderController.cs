using DataContract;
using DataContract.Order;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;
using ServiceLayer.Carts;
using ServiceLayer.Helper;
using ServiceLayer.Order;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly WhatsAppHelper _whatsAppHelper;
        private readonly CartService _cartservice;
        public OrderController(OrderService orderService, WhatsAppHelper whatsAppHelper,CartService cartService)
        {
            _cartservice = cartService;
            _orderService = orderService;
            _whatsAppHelper = whatsAppHelper;
        }
        [HttpPost]
       
        public async Task<IActionResult> PlaceOrder(CheckOutOrderDC checkOutOrderDC)
        {
            
            long res = await _orderService.SubmitOrder(checkOutOrderDC);
            if ( res>0)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("Something went wrong please try again later!!");
            }
        }

        [HttpGet] 
        public async Task<IActionResult> GetCart(string mongoid)
        {
            

           var res = await _cartservice.GetCartForUser(mongoid);
            return Ok(res);
        }

        //[HttpGet]
        //public IActionResult SendSMs()
        //{
        //    _whatsAppHelper.SendSMS();
        //    return Ok(true);
        //}
    }
}
