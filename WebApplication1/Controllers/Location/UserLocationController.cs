using DataContract.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Address;

namespace Suppliment.API.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserLocationController : ControllerBase
    {
        public UserLocationService _userLocationService;
        public UserLocationController(UserLocationService userLocationService)
        {
            _userLocationService = userLocationService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitUserLocation(UserLocationDC userLocationDC)
        {
            var data = await _userLocationService.SubmitUserLocation(userLocationDC);
            return Ok(data);
        }
    }
}
