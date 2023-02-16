using DataContract.Delivery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Delivery;
using ServiceLayer.Helper;

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

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var data = await _deliveryService.Authenticate();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> GetServicable(ServiceableRequestDC serviceableRequestDC)
        {
            var data = await _deliveryService.GetServicable(serviceableRequestDC);
            return Ok(data);
        }
    }
}
