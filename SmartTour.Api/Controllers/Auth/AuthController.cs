using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.Auth.PassswordChangeDtos;
using SmartTour.Business.Enums;
using SmartTour.Business.Services.Auth.Abstract;
using SmartTour.Business.Services.Auth.Concrete;
using System.Security.Claims;

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

        //New:

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswortReguestDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto.Email);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetRegusestDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);

            if(!result) return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordReguestDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _authService.ChangePasswordAsync(
                userId,
                dto.CurrentPassword,
                dto.NewPassword
                );
            if (!result) return BadRequest("Current password is incorrect");
            return Ok();
        }



    }
}
