using DataContract.Address;
using DataContract.Delivery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Address;
using ServiceLayer.Delivery;
using ServiceLayer.Helper;

namespace Suppliment.API.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PincodeController : ControllerBase
    {
        private readonly PinCodeService _pincodeService;
      
        public PincodeController(PinCodeService pinCodeService, ShippingRocketHelper shippingRocketHelper)
        {
            _pincodeService = pinCodeService;
            
        }

        [HttpGet]

        public async Task<IActionResult> GetDetailsbyPincode(string pincode)
        {
            var data = await _pincodeService.GetDetailsbyPincode(pincode);
            return Ok(data);
        }

        [HttpPost]

        public async Task<IActionResult> GetEtd(EtdRequestDC etdRequestDC )
        {
            var data = await _pincodeService.Getetd(etdRequestDC);
            return Ok(data);
        }
    }
}
