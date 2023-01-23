using DataContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface.IService;

namespace Suppliment.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IAuthService    _iauthService ;
        
        public UserController(IAuthService iauthService) { 
        _iauthService= iauthService;
        }
        [HttpPost]
        public IActionResult Authentication(UserCredentialDC logincredentail)
        {
            var result =  _iauthService.Token(logincredentail);

            if (result==null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok( result);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer",Roles ="Admin")]

        [HttpGet]
        public IActionResult GetUserName()
        {
            string username = _iauthService.GetUserName();
            return Ok(username);

        }
    }
}
