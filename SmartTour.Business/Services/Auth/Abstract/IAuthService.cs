
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.GogleDto;
using SmartTour.Business.Enums;
using SmartTour.Entities.Users;

namespace SmartTour.Business.Services.Auth.Abstract
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto dto);

        // Login → token + user info
        Task<(LoginStatus status,string? token,  Guid? userId, int? expiresIn)> LoginAsync(LoginRequestDto dto);

//###################################################

        Task<(LoginStatus status,string? token, Guid? userId, int? expiresIn)>
            GoogleLoginAsync(GoogleUserDto dto);

        //new:
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string token, string newPassword);
        Task<bool> ChangePasswordAsync(Guid userId,string currentPassword,string newPassword);
    }



}
