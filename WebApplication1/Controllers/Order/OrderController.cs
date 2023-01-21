using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;
using ServiceLayer.Interface.IHelper;
using ServiceLayer.Order;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public OrderService _orderService;
        public AuthService _authService;
        public OrderController(OrderService orderService, AuthService authService)
        {

            _orderService = orderService;
            _authService = authService;
        }
        [HttpPost]

        public async Task<IActionResult> PlaceOrder(List<OrderDetailDC> orderdetailDc)
        {
            long userid = _authService.GetUserId();
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
    }
}
