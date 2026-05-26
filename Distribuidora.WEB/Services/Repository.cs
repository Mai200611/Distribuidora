using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace Distribuidora.WEB.Services
{
    public class Repository : IRepository
    {
        private readonly HttpClient _http;
        private readonly ITokenService _tokenService;

        public Repository(HttpClient http, ITokenService tokenService)
        {
            _http = http;
            _tokenService = tokenService;
        }

        private async Task SetAuthHeader()
        {
            var token = await _tokenService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            await SetAuthHeader();
            var response = await _http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new HttpResponseWrapper<T>(result, false, response);
            }
            return new HttpResponseWrapper<T>(default, true, response);
        }

        public async Task<HttpResponseWrapper<T>> Post<T>(string url, T model)
        {
            await SetAuthHeader();
            var response = await _http.PostAsJsonAsync(url, model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new HttpResponseWrapper<T>(result, false, response);
            }
            return new HttpResponseWrapper<T>(default, true, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest model)
        {
            await SetAuthHeader();
            var response = await _http.PostAsJsonAsync(url, model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TResponse>();
                return new HttpResponseWrapper<TResponse>(result, false, response);
            }
            return new HttpResponseWrapper<TResponse>(default, true, response);
        }

        public async Task<HttpResponseWrapper<T>> Put<T>(string url, T model)
        {
            await SetAuthHeader();
            var response = await _http.PutAsJsonAsync(url, model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new HttpResponseWrapper<T>(result, false, response);
            }
            return new HttpResponseWrapper<T>(default, true, response);
        }

        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            await SetAuthHeader();
            var response = await _http.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !response.IsSuccessStatusCode, response);
        }
    }
}
