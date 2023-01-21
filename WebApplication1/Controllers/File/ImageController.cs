using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.File;
using ServiceLayer.Interface.IService;

namespace Suppliment.API.Controllers.File
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public readonly  IFileService _fileService;
        public ImageController(IFileService fileService) {
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
