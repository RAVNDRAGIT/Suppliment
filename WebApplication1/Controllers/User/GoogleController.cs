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
        public IActionResult Authentication(GooglePayloadDc googlePayloadDc)
        {
            try
            {
                var result = _googleService.ValidateGoogleToken(googlePayloadDc);


                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }
}
