using BusinessLayer;
using DataContract.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Helper;
using ServiceLayer.Product;
using Suppliment.API.Model;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService  _productMasterService;
        
         public ProductController(ProductService productMasterService, JwtMiddleware jwtMiddleware )
        {
            _productMasterService = productMasterService;
            
        }
        [HttpGet]
        public async Task<IActionResult>AddProductAsync(ProductMasterDC productMasterDC)
        {
           
            bool result= await _productMasterService.AddProduct(productMasterDC);
           
            if(result)
            {
                return Ok("Product Added Succesfully!");
            }
            else
            {
                return BadRequest("Something went wrong plz try again later!!");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> AddExcelProduct(IFormFile file)
        {

            bool result = await _productMasterService.AddExcelProduct();

            if (result)
            {
                return Ok("Product Added Succesfully!");
            }
            else
            {
                return BadRequest("Something went wrong plz try again later!!");
            }
        }
    }
}
