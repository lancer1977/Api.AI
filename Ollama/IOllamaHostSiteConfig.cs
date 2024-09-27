namespace Ollama;

public interface IOllamaHostSiteConfig
{
    public string Url { get; set; }
    public string WebKey { get; set; }
}