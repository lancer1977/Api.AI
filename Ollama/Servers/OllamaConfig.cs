namespace PolyhydraGames.Ollama.Servers;

public class OllamaConfig : IOllamaConfig
{
    public string ApiUrl { get; set; } 
    public string Background { get; set; }
    public string DefaultModel { get; set; } = "llama3.1:latest";
}