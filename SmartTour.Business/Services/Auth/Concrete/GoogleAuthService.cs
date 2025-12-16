using Microsoft.Extensions.Configuration;
using SmartTour.Business.DTOs.Auth;
using SmartTour.Business.DTOs.GogleDto;
using SmartTour.Business.Services.Auth.Abstract;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SmartTour.Business.Services.Auth.Concrete
{
    public class GoogleAuthService : IGoogleService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<GoogleUserDto> GetUserAsync(string code)
        {
            var tokenResponse = await _httpClient.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", _configuration["Google:ClientId"]! },
                    { "client_secret", _configuration["Google:ClientSecret"]! },
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", _configuration["Google:RedirectUri"]! }
                })
            );
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenJson);

            var accessToken = tokenData.GetProperty("access_token").GetString();

            // 2️⃣ User info al
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await _httpClient.GetAsync(
                "https://www.googleapis.com/oauth2/v3/userinfo"
            );

            var userJson = await userInfoResponse.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(userJson);

            return new GoogleUserDto
            {
                Email = userData.GetProperty("email").GetString()!,
                FullName = userData.GetProperty("name").GetString()!,
                GoogleId = userData.GetProperty("sub").GetString()!,
                AvatarUrl = userData.TryGetProperty("picture", out var pic)
                    ? pic.GetString()
                    : null
            };
        }
    }
}
