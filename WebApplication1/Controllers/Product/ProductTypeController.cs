using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Home;
using ServiceLayer.Product;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductTypeController : ControllerBase
    {
        private readonly ProductTypeService _productTypeService;
        public ProductTypeController(ProductTypeService productTypeService) {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductTypeList()
        {
            var data= await _productTypeService.GetProductTypeList();
            return Ok(data);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Save(IFormFile file)
        {
            var data = await _productTypeService.Save();
            return Ok(data);
        }
    }
}
