namespace PolyhydraGames.Ollama;

public interface IOllamaConfig
{
    public string ApiUrl { get; set; }

    public string Background { get; set; }
    public string DefaultModel { get; set; }
}

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);