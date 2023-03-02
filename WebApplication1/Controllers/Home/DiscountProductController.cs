using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Product;

namespace Suppliment.API.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
 
    [Produces("application/json")]
    public class DiscountProductController : ControllerBase
    {
        private readonly ProductService _productMasterService;
        public DiscountProductController(ProductService productMasterService)
        {
            _productMasterService = productMasterService;
        }
        [HttpGet]
        public async Task<IActionResult> GetDiscountProduct(int skip, int take)
        {
            var data = await _productMasterService.GetDiscountProduct(skip, take);
            return Ok(data);
        }
    }
}
