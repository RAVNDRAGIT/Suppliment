using DataContract.Cart;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Carts;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AssignCart : ControllerBase
    {
        public CartService _cartService;
        public AssignCart(CartService cartService) {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Cart(AssignCartDC assignCartDC)
        {
            var result= await _cartService.AssignCartAsync(assignCartDC);
            return Ok (result);

        }

    }
}
