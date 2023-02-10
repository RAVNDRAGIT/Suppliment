using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Home;

namespace Suppliment.API.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GoalController : ControllerBase
    {
        private readonly GoalService _goalService;
        public GoalController(GoalService goalService) {
            _goalService = goalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveGoals()
        {
            var data= await _goalService.GetActiveGoals();
            return Ok(data);
        }
    }
}
