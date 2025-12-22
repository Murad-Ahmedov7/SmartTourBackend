using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.Auth.PassswordChangeDtos;

using SmartTour.Business.Enums;
using SmartTour.Business.ExternalAuth;
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
                AuthProvider = AuthProviderType.Local
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

        public async Task<(string token, Guid userId, int expiresIn)> LoginWithGoogleAsync(GoogleUserInfo info)
        {
            var user = await _userRepository.GetByGoogleIdAsync(info.GoogleId);

            if (user == null)
            {
                user = new User
                {
                    FullName = info.FullName,
                    Email = info.Email,
                    GoogleId = info.GoogleId,
                    AvatarUrl = info.AvatarUrl,
                    AuthProvider = AuthProviderType.Google
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user);
            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]!);

            return (token, user.Id, expireMinutes * 60);
        }







        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return;

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(10);

            await _userRepository.SaveChangesAsync();


            var frontendBaseUrl = _configuration["Frontend:BaseUrl"];
            var resetLink = $"http://localhost:5173/reset-password?token={user.PasswordResetToken}";

            Console.WriteLine($"RESET LINK: {resetLink}");
            var body = $@"
            <h2>Password Reset</h2>
            <p>We received a request to reset your password.</p>
            <p>
                <a href='{resetLink}'>Reset Password</a>
            </p>
            <p>This link will expire in 10 minutes.</p>
            <p>If you did not request this, you can ignore this email.</p>
            ";

            try
            {
                await _emailService.SendAsync(user.Email, "Reset password", body);
            }
            catch (Exception ex)
            {
                // LOG yaz
                Console.WriteLine(ex.Message);
                // BURADA STOP. Exception yuxarı atılmır
            }
        }

        public async Task<ResetPasswordStatus> ResetPasswordAsync(string token, string newPassword)
        {
            var user = await _userRepository.FindByResetTokenAsync(token);

            if (user == null) return ResetPasswordStatus.InvalidToken;

            if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;

                await _userRepository.SaveChangesAsync();

                return ResetPasswordStatus.TokenExpired;
            }
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 8)
                return ResetPasswordStatus.PasswordInvalid;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _userRepository.SaveChangesAsync();


            return ResetPasswordStatus.Success;

        }

        public async Task<ChangePasswordStatus> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null) return ChangePasswordStatus.UserNotFound;

            var isValid = BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash);

            if (!isValid) return ChangePasswordStatus.WrongPassword;    

            var isSame = BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.PasswordHash);

            if (isSame) return ChangePasswordStatus.PasswordUnchanged;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _userRepository.SaveChangesAsync();


            //if (user.AuthProvider == AuthProvider.Google)
            //    return ChangePasswordStatus.GoogleAccount; Google Accounta gore olan halini sonra duzgun yaz.

            return ChangePasswordStatus.Success;
        }









    }
}
