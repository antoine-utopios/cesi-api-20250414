using Demo.BruteForce.Models;
using Demo.BruteForce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.BruteForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestPayload payload)
        {
            try
            {
                var response = _authService.Login(payload);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestPayload payload)
        {
            try
            {
                var response = _authService.Register(payload);
                return Ok(new
                {
                    Message = "User created",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("unlock/{username}")]
        public IActionResult Unlock(string username)
        {
            try
            {
                _authService.UnlockAccount(username);
                return Ok(new
                {
                    Message = "User unlocked",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
