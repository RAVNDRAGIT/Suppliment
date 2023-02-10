using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Home;

namespace Suppliment.API.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService ;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveGoals()
        {
            var data = await _categoryService.GetActiveCategories();
            return Ok(data);
        }
    }
}
