using Distribuidora.Shared.DTOs.Auth;
using System.Net.Http.Json;


namespace Distribuidora.WEB.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthResponseDTO?> Login(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync(
                "https://localhost:5096/api/auth/login",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content
                .ReadFromJsonAsync<AuthResponseDTO>();
        }
    }
}