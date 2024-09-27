using System.Net.Http.Json;
using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama.Host;

public class OllamaHostApi :IOllamaHostApi
{
    public HttpClient _client = new HttpClient();
    public OllamaHostApi(IOllamaHostSiteConfig config)
    {
            Config = config;
            _client.DefaultRequestHeaders.Add("streamerid", Config.WebKey);
        }

    public IOllamaHostSiteConfig Config { get; set; }

    public async Task<bool> AddMessage(AddMessageParams message)
    { 
            var endpoint = Config.Url + "/api/ai/addmessage";
            var content =  JsonContent.Create(message);
            var response = await _client.PostAsync(endpoint, content);
            var ok = response.IsSuccessStatusCode;
            return ok;
        }
    public async Task<bool> Impersonate(AddMessageParams message)
    {
            var endpoint = Config.Url + "/api/ai/impersonate";
            var content = JsonContent.Create(message);
            var response = await _client.PostAsync(endpoint, content);
            var ok = response.IsSuccessStatusCode;
            return ok;
        }
}