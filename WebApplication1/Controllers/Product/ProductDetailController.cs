
using DataContract.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Product;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductDetailController : ControllerBase
    {
        private readonly ProductService _productMasterService;
        public ProductDetailController(ProductService productMasterService)
        {
            _productMasterService = productMasterService;
        }
        [HttpPost]
        public async Task<IActionResult> GetProductDynamically(ProductFilterDC productFilterDC)
        {

            var data = await _productMasterService.GetProductDynamically(productFilterDC);
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetLikeProduct(long producttypeid, long productid)
        {
            var data = await _productMasterService.GetLikeProduct(producttypeid, productid);
            return Ok(data);
        }
    }
}
