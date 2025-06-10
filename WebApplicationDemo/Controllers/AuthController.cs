using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplicationDemo.Data;
using WebApplicationDemo.DTOs;
using WebApplicationDemo.Model;
using WebApplicationDemo.Services;

namespace WebApplicationDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var passwordHash = ComputeSha256Hash(dto.Password);
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == dto.Username && u.PasswordHash == passwordHash);

            if (user == null)
            {
                return Unauthorized("用户名或密码错误");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // 用户名是否已存在
            if (_context.Users.Any(u => u.Username == dto.Username))
            {
                return BadRequest("用户名已存在");
            }

            // 加密密码
            var passwordHash = ComputeSha256Hash(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Role = dto.Role // 保存角色
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("注册成功");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes);
        }
    }
}
