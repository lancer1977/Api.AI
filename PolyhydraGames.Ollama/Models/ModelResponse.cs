using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;

public class ModelResponse
{
    [JsonPropertyName("models")]
    public List<ModelDetail> Models { get; set; }
}