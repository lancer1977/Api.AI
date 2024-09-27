namespace PolyhydraGames.Ollama.Ollama;

public class OllamaConfig : IOllamaConfig
{
    public string ApiUrl { get; set; }

    public string Key { get; set; }
    public string Background { get; set; }
}