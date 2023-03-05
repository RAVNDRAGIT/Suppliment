using DataContract.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Helper;
using ServiceLayer.Product;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductImageController : ControllerBase
    {
        private readonly ProductImageService _productImageService;

        public ProductImageController(ProductImageService productImageService)
        {
            _productImageService = productImageService;

        }

        [HttpGet]
        public async Task<IActionResult> GetProductImage(long productid)
        {

            var data  = await _productImageService.GetProductImageList(productid);
            return Ok(data);
            
        }
    }
}
