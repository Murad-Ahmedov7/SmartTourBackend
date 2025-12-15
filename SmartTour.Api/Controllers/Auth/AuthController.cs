using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.Enums;
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

            if (result.status == LoginStatus.Success)
                return Ok(new { result.token, result.userId, result.expiresIn });

            if (result.status == LoginStatus.Locked)
                return StatusCode(429,
                    new { message = "Çox sayda uğursuz giriş cəhdi aşkarlandı. Zəhmət olmasa, bir müddət sonra yenidən cəhd edin." });

            if (result.status == LoginStatus.InvalidCredentials)
                return Unauthorized(new { message = "Email və ya şifrə yanlışdır." });

            // FALLBACK — bu artıq backend səhvidir
            return StatusCode(500, new { message = "Gözlənilməz xəta baş verdi." });

        }

     

    }
}
