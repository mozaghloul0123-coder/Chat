using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Div.Link.Project01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            return Ok(new { Message = "Welcome to the Admin Dashboard!", Stats = new { Users = 150, Revenue = 5000 } });
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(new[] { "User1", "User2", "Admin1" });
        }
    }
}
