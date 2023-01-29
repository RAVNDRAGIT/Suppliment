using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Helper;

namespace Suppliment.API.Controllers.WhatsApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : ControllerBase
    {
        private readonly WhatsAppHelper _whatsAppHelper;
        public WhatsAppController(WhatsAppHelper whatsAppHelper)
        {
            _whatsAppHelper = whatsAppHelper;
        }

        [HttpGet]
        public async Task<IActionResult> SendSms(long orderid)
        {
            var data = await _whatsAppHelper.SendMessgae(orderid);
            return Ok(data);
        }
    }
}
