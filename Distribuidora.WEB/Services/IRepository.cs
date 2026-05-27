using System.Text.Json;

namespace Distribuidora.WEB.Services
{
    public class HttpResponseWrapper<T>
    {
        public bool Error { get; set; }
        public T? Response { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error) return null;
            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == System.Net.HttpStatusCode.NotFound) return "Registro no encontrado.";
            if (statusCode == System.Net.HttpStatusCode.BadRequest) return await HttpResponseMessage.Content.ReadAsStringAsync();
            if (statusCode == System.Net.HttpStatusCode.Unauthorized) return "No autorizado.";
            return "Error inesperado.";
        }
    }

    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest model);
        Task<HttpResponseWrapper<T>> Put<T>(string url, T model);
        Task<HttpResponseWrapper<object>> Delete(string url);
    }
}
