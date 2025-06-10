using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplicationDemo.Model;

namespace WebApplicationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("boom")]
        public IActionResult ThrowException()
        {
            throw new Exception("模拟异常");
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var username = User.Identity?.Name;

            // 从 Claims 中获取用户 ID（如果需要）
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                UserId = userId,
                Username = username
            });
        }
    }
}
