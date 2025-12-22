
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.Auth.PassswordChangeDtos;
using SmartTour.Business.Enums;
using SmartTour.Business.ExternalAuth;
using SmartTour.Entities.Users;

namespace SmartTour.Business.Services.Auth.Abstract
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto dto);

        // Login → token + user info
        Task<(LoginStatus status, string? token, Guid? userId, int? expiresIn)> LoginAsync(LoginRequestDto dto);



        Task<(string token, Guid userId, int expiresIn)> LoginWithGoogleAsync(GoogleUserInfo info);


        Task ForgotPasswordAsync(string email);
        Task<ResetPasswordStatus> ResetPasswordAsync(string token, string newPassword);
        Task<ChangePasswordStatus> ChangePasswordAsync(Guid userId,ChangePasswordRequestDto dto);


    }



}
