using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Helper;
using ServiceLayer.Order;

namespace Suppliment.API.Controllers.WhatsApp
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WhatsAppController : ControllerBase
    {
        private readonly OrderService _orderService;
        public WhatsAppController(OrderService orderService )
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> SendSms(string resorderid)
        {
            var data = await _orderService.UpdateWhatsAppStatus(resorderid);
            return Ok(data);
        }
    }
}
