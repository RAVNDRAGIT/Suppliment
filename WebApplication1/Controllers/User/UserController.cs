using DataContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;

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
        public IActionResult Authentication(UserCredentialDC userCredentialDC)
        {
            string result =  _iauthService.Token(userCredentialDC);

            if (result==null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
