using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.Auth.PassswordChangeDtos;
using SmartTour.Business.Enums;
using SmartTour.Business.ExternalAuth;
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

            return StatusCode(500, new { message = "Gözlənilməz xəta baş verdi." });

        }



        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/google-success"
            };

            return Challenge(properties, "Google");
        }


        [AllowAnonymous]
        [HttpGet("google-success")]
        public async Task<IActionResult> GoogleSuccess()
        {
            var result = await HttpContext.AuthenticateAsync("Cookies");

            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal!.Claims;

            var googleUser = new GoogleUserInfo
            {
                GoogleId = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                Email = claims.First(x => x.Type == ClaimTypes.Email).Value,
                FullName = claims.First(x => x.Type == ClaimTypes.Name).Value,
                AvatarUrl = claims.FirstOrDefault(x => x.Type == "picture")?.Value
            };

            var (token, userId, expiresIn) =
                await _authService.LoginWithGoogleAsync(googleUser);

            return Ok(new { token, userId, expiresIn });
        }





        //New:

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswortRequestDto dto)
        {
            await _authService.ForgotPasswordAsync(dto.Email);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetRequestDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);


            if (result == ResetPasswordStatus.InvalidToken) return BadRequest("Invalid reset token");

            if (result == ResetPasswordStatus.TokenExpired) return BadRequest("Reset token expired");

            if (result == ResetPasswordStatus.PasswordInvalid) return BadRequest("Password is invalid");

            if (result == ResetPasswordStatus.Success)  return Ok("Password reset successfully");

            return StatusCode(500, "Unexpected error");

          
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if(!Guid.TryParse(userIdString,out var userId)) return Unauthorized();

            var result = await _authService.ChangePasswordAsync(userId, dto);

            if (result == ChangePasswordStatus.UserNotFound) return NotFound();

            if (result == ChangePasswordStatus.WrongPassword) return BadRequest("Current password is incorrect");

            if (result == ChangePasswordStatus.PasswordUnchanged) return BadRequest("New password must be different from the old password");

            //if (result == ChangePasswordStatus.GoogleAccount)
            //    return BadRequest("Password cannot be changed for Google accounts");

            if (result != ChangePasswordStatus.Success) return StatusCode(500, "Unexpected error occurred");

            return Ok();


        }



    }
}
