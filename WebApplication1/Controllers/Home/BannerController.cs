using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Home;

namespace Suppliment.API.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BannerController : ControllerBase
    {
        private readonly BannerService _bannerService;
        public BannerController(BannerService bannerService) { 
        _bannerService= bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBannerList()
        {
            var data = await _bannerService.GetBannerList();
            return Ok(data);
        }
    }
}
