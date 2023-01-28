using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Payment;

namespace Suppliment.API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;

        }

        [HttpGet]
        public async Task<IActionResult> CreateOrderAsync(long orderid)
        {
            var res= await _paymentService.CreateOrderAsync(orderid);
            return Ok(res);
        }

        //[HttpGet]
        //public async Task<IActionResult> CapturePaymentAsync(string orderId)
        //{
        //    var res = await _paymentService.CapturePayment(orderId);
        //    return Ok(res);
        //}
    }
}
