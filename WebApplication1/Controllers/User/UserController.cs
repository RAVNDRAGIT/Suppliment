using DataContract.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;

namespace Suppliment.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly AuthService _authService ;
        

        public UserController(AuthService iauthService) {
            _authService = iauthService;
        }
        [HttpPost]
        public IActionResult Authentication(UserCredentialDC logincredentail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _authService.Token(logincredentail);

            if (result==null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok( result);
            }
        }

        //[Authorize(AuthenticationSchemes = "Bearer",Roles ="Admin")]

        //[HttpGet]
        //public IActionResult GetUserName()
        //{
        //    string username = _iauthService.GetUserName();
        //    return Ok(username);

        //}
    }
}
