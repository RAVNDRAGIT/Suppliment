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
        public async Task<IActionResult> UpdatePaymentStatus(string resorderid)
        {
            var res= await _paymentService.AfterPayment(resorderid);
            return Ok(res);

        }
    }
}
