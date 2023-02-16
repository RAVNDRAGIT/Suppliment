using DataContract.Address;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Address;

namespace Suppliment.API.Controllers.Location
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserLocationController : ControllerBase
    {
        private readonly  UserLocationService _userLocationService;
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

         [HttpGet]
        public async Task<IActionResult> GetUserAddressbyuserId()
        {
            var data = await _userLocationService.GetUserAddressbyUserid();
            return Ok(data);
        }

    }
}
