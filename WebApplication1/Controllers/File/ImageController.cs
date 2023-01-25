using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.File;

namespace Suppliment.API.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public readonly FileService _fileService;
        public ImageController(FileService fileService) {
            _fileService=fileService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            string res= await _fileService.SaveImageAsync(file.FileName);
            if(!string.IsNullOrEmpty(res))
            {
                return Ok( res);
            }
            else
            {
                return BadRequest("Something went wrong please try again later!!");
            }
        }
    }
}
