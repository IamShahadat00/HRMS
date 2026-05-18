using HRMS.Core.DTOs;
using HRMS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginRequest)
        {
            var result = await authService.LoginAsync(loginRequest);
            if (result == "User not found" || result == "Invalid password")
            {
                return Unauthorized(new { message = result });
            }
            return Ok(new { token = result });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerRequest)
        {
            var result = await authService.RegisterAsync(registerRequest);
            if (result == "User already exists" || result == "Password do not match")
            {
                return BadRequest(new { message = result });
            }
            return Ok(new { message = result });
        }
    }
}
