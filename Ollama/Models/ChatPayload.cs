using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;

public class ChatPayload
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; }

    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}