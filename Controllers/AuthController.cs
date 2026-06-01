using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailOptimizationPlatform.DTOs;
using RetailOptimizationPlatform.Models;
using RetailOptimizationPlatform.Services;

namespace RetailOptimizationPlatform.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IJwtService jwtService, IWebHostEnvironment environment)
        {
            _jwtService = jwtService;
            _environment = environment;
        }

        [AllowAnonymous]
        [HttpPost("dev-token")]
        public IActionResult GetDevToken([FromBody] DevTokenRequest? request)
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound(new { message = "This endpoint is available only in Development." });
            }

            var user = new AppUser
            {
                AppUserId = 1,
                Email = request?.Email ?? "admin@retail.local",
                Role = string.IsNullOrWhiteSpace(request?.Role) ? "Admin" : request!.Role,
                FullName = "Development Admin",
                PasswordHash = string.Empty
            };

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}
