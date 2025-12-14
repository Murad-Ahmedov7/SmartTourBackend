
using SmartTour.Business.DTOs.Auth;
using SmartTour.Entities.Users;

namespace SmartTour.Business.Services.Auth.Abstract
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto dto);

        // Login → token + user info
        Task<(string token, Guid userId, int expiresIn)?> LoginAsync(LoginRequestDto dto);
    }
}
