using DataContract.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;

namespace Suppliment.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class GoogleController : ControllerBase
    {
        private readonly GoogleService _googleService;
        public GoogleController(GoogleService googleService)
        {
            _googleService = googleService;
        }

        [HttpPost]
        public async Task <IActionResult> Authentication(GooglePayloadDc googlePayloadDc)
        {
           
                var result = await _googleService.ValidateGoogleToken(googlePayloadDc);


                return Ok(result);
            

        }

    }
}
