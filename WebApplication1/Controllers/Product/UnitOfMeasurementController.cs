using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Product;

namespace Suppliment.API.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasurementController : ControllerBase
    {
        private readonly UomService _uomService;
        public UnitOfMeasurementController(UomService uomService) {
            _uomService = uomService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Save(IFormFile file)
        {
            var res = await _uomService.Save();
            return Ok(res); 
        }
    }
}
