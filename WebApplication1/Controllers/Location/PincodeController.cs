using DataContract.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Address;

namespace Suppliment.API.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PincodeController : ControllerBase
    {
        public PinCodeService _pincodeService;
        public PincodeController(PinCodeService pinCodeService)
        {
            _pincodeService = pinCodeService;
        }

        [HttpGet]

        public async Task<IActionResult> GetDetailsbyPincode(string pincode)
        {
            var data = await _pincodeService.GetDetailsbyPincode(pincode);
            return Ok(data);
        }
    }
}
