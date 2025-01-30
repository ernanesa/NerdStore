using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApp.MVC.Extensions;
using WebApp.MVC.Models;


namespace WebApp.MVC.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _httpClient;

        public AutenticacaoService(IOptions<AppSettings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_settings.AutenticacaoUrl);
        }
        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(usuarioLogin), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return response.IsSuccessStatusCode ? JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options) : null;
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(JsonSerializer.Serialize(usuarioRegistro), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/identidade/nova-conta", registroContent);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return response.IsSuccessStatusCode ? JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options) : null;
        }
    }
}