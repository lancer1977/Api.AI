using System.Text.Json.Serialization;

namespace Ollama.Models;

public class ModelResponse
{
    [JsonPropertyName("models")]
    public List<ModelDetail> Models { get; set; }
}