using DataContract.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Payment;

namespace Suppliment.API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PaymentStatusController : ControllerBase
    {
        public PaymentService _paymentService;
        public PaymentStatusController(PaymentService paymentService ) {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePaymentStatus(long orderid)
        {
            var res= await _paymentService.AfterPayment(orderid);
            return Ok(res);

        }
    }
}
