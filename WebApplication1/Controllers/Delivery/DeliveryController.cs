using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Delivery;

namespace Suppliment.API.Controllers.Delivery
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DeliveryController : ControllerBase
    {
        public readonly DeliveryService _deliveryService;
        public DeliveryController(DeliveryService deliveryService) {
            _deliveryService = deliveryService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken(long orderid)
        {
            var data = await _deliveryService.Authenticate(orderid);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetServicable(long orderid)
        {
            var data = await _deliveryService.GetServicable(orderid);
            return Ok(data);
        }
    }
}
