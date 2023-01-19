using BusinessLayer;
using BusinessLayer.ProductMaster;
using DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using ServiceLayer.Interface;
using Suppliment.API.Model;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductMasterService  _productMasterService;
       
        public ProductController(IProductMasterService productMasterService )
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
