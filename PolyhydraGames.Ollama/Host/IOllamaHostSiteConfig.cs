namespace PolyhydraGames.Ollama.Host
{
    public interface IOllamaHostSiteConfig
    {
        public string Url { get; set; }
        public string WebKey { get; set; }
    }

    public class OllamaHostSiteConfig : IOllamaHostSiteConfig
    {
        public string Url { get; set; }
        public string WebKey { get; set; }
    }

    public class OllamaHostApi :IOllamaHostApi
    {
        public HttpClient _client = new HttpClient();
        public OllamaHostApi(IOllamaHostSiteConfig config)
        {
            Config = config;
            _client.DefaultRequestHeaders.Add("streamid", Config.WebKey);
        }

        public IOllamaHostSiteConfig Config { get; set; }

        public async Task<bool> AddMessage(string message)
        { 
                var endpoint = Config.Url + "/api/ai/AddMessage";
                var content = new StringContent(message);
                var response = await _client.PostAsync(endpoint, content);
                var ok = response.IsSuccessStatusCode;
                return ok;
        }
    }

    public interface IOllamaHostApi
    {
        Task<bool> AddMessage(string message);
    }
}