namespace PolyhydraGames.Ollama.Host
{
    public class OllamaHostSiteConfig : IOllamaHostSiteConfig
    {
        public string Url { get; set; }
        public string WebKey { get; set; }
    }
}