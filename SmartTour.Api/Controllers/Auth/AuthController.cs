using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.Enums;
using SmartTour.Business.Services.Auth.Abstract;
using SmartTour.Business.Services.Auth.Concrete;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartTour.Api.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IGoogleService _googleAuthService;

        public AuthController(IAuthService authService, IGoogleService googleAuthService)
        {
            _authService = authService;
            _googleAuthService = googleAuthService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            await _authService.RegisterAsync(dto);
            return Ok(dto);

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

            return StatusCode(500, new { message = "Gözlənilməz xəta baş verdi." });

        }


        //#############################################
        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallBack(string code)
        {
            // 1️⃣ Google-dan user al
            var googleUser = await _googleAuthService.GetUserAsync(code);

            // 2️⃣ AuthService-ə ötür
            var result = await _authService.GoogleLoginAsync(googleUser);

            return Ok(new
            {
                result.token,
                result.userId,
                result.expiresIn
            });
        }




    }
}
