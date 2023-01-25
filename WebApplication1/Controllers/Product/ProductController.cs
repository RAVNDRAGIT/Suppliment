using BusinessLayer;
using BusinessLayer.ProductMaster;
using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Product;
using Suppliment.API.Model;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly ProductService  _productMasterService;
       
        public ProductController(ProductService productMasterService )
        {
            _productMasterService = productMasterService;
        }
        [HttpPost]
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

       
    }
}
