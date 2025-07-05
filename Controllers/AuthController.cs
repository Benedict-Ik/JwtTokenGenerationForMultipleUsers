using JwtTokenGenerationForMultipleUsers.Interfaces;
using JwtTokenGenerationForMultipleUsers.Models;
using JwtTokenGenerationForMultipleUsers.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenGenerationForMultipleUsers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            this._userService = userService;
            this._jwtService = jwtService;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.DTOs.RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = new User { Username = request.Username };
            var result = await _userService.CreateUserAsync(user, request.Password);

            if (result == null)
            {
                return StatusCode(500, "Error creating user.");
            }

            return Ok(result);
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.DTOs.LoginRequest request)
        {
            var user = await _userService.GetUserAsync(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new
            {
                message = $"{user.Username} has successfully logged in.",
                token = token
            });

        }
    }
}
