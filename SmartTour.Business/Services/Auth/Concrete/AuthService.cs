using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.GogleDto;
using SmartTour.Business.Enums;
using SmartTour.Business.Services.Auth.Abstract;
using SmartTour.DataAccess.Repositories.Auth.Abstract;
using SmartTour.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartTour.Business.Services.Auth.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        // ================= CONSTRUCTOR =================
        public AuthService(IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        // ================= REGISTER =================
        public async Task<bool> RegisterAsync(RegisterRequestDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = passwordHash,
                AuthProvider = AuthProviderType.Local.ToString()
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // ================= LOGIN =================
        public async Task<(LoginStatus status, string? token, Guid? userId, int? expiresIn)>
            LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return (LoginStatus.InvalidCredentials, null, null, null);

            // Lockout bitibsə → təmizlə
            if (user.LockoutUntil.HasValue && user.LockoutUntil.Value <= DateTime.UtcNow)
            {
                user.LockoutUntil = null;
                user.FailedLoginAttempts = 0;
                await _userRepository.SaveChangesAsync();
            }

            // Hələ də blokdadırsa
            if (user.LockoutUntil.HasValue && user.LockoutUntil.Value > DateTime.UtcNow)
                return (LoginStatus.Locked, null, null, null);

            var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordValid)
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 5 && user.LockoutUntil == null)
                    user.LockoutUntil = DateTime.UtcNow.AddMinutes(15);

                await _userRepository.SaveChangesAsync();
                return (LoginStatus.InvalidCredentials, null, null, null);
            }

            // Uğurlu login
            user.FailedLoginAttempts = 0;
            user.LockoutUntil = null;
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

            return (LoginStatus.Success, token, user.Id, expireMinutes * 60);
        }


        // ================= JWT GENERATION =================
        private string GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");

            var keyString = jwtSection["Key"]
                ?? throw new Exception("JWT Key is missing in appsettings.json");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyString)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>
            {
                // JWT standard
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64),

                // ASP.NET Core üçün rahatlıq
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var expireMinutes = int.Parse(jwtSection["ExpireMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //#####################GOOGLE LOGIN ##############################

        public async Task<(LoginStatus status, string? token, Guid? userId, int? expiresIn)>
    GoogleLoginAsync(GoogleUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = dto.Email,
                    FullName = dto.FullName,
                    GoogleId = dto.GoogleId,
                    AvatarUrl = dto.AvatarUrl,
                    AuthProvider = AuthProviderType.Google.ToString(),
                    LastLogin = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }
            else
            {
                if (user.AuthProvider == AuthProviderType.Local.ToString())
                {
                    user.GoogleId = dto.GoogleId;
                    user.AuthProvider = AuthProviderType.Google.ToString();
                }

                user.LastLogin = DateTime.UtcNow;
                await _userRepository.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user);
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

            return (LoginStatus.Success, token, user.Id, expireMinutes * 60);
        }
        //#########################CHANGE PASSWORD########################

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return false;

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);

            await _userRepository.SaveChangesAsync();

            var resetLink =
                $"https://frontend-url/reset-password?token={user.PasswordResetToken}";

            var body = $@"
        <h2>Password Reset</h2>
        <p>Click the link below to reset your password:</p>
        <a href='{resetLink}'>Reset Password</a>
        <p>This link will expire in 30 minutes.</p>
    ";

            await _emailService.SendAsync(
                user.Email,
                "Reset your password",
                body
            );

            return true;
        }


        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.GetByResetTokenAsync(token);
            if (user == null) return false;
            if (user.PasswordResetTokenExpiry < DateTime.UtcNow) return false;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash)) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.SaveChangesAsync();
            return true;
        }




    }
}
