using Microsoft.JSInterop;

namespace Distribuidora.WEB.Services
{
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime _js;
        private const string TokenKey = "TOKEN_KEY";

        public TokenService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }

        public async Task RemoveTokenAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
    }
}
