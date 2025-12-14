using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.Services.Auth.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartTour.Api.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok();

        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                token = result.Value.token,
                user_id = result.Value.userId,
                expires_in = result.Value.expiresIn
            });
        }

    }
}
