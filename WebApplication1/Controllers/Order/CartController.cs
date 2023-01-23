using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Auth;
using ServiceLayer.Interface.IService;
using ServiceLayer.Mongo;

namespace Suppliment.API.Controllers.Order
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public CartService _cartService { get; set; }
        
       
        public CartController(CartService cartService) {
            _cartService = cartService;
            
        }

        [HttpPost]
        public async Task<IActionResult> AddCart(CartDetailDC cartDetailDC)
        {
            string res = await _cartService.CreateAsync(cartDetailDC);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart(string cookieValue)
        {
            var res = await _cartService.GetAsync(cookieValue);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveCart(string cookieValue)
        {
            bool res = await _cartService.RemoveAsync(cookieValue);
            return Ok(res);
        }
    }
}
