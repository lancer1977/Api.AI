using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;

public class OllamaResponse : BaseResponse
{
    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("context")]
    public List<int>? Context { get; set; }



}
