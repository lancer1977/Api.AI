using System.Text.Json.Serialization;
using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public interface IAIService
{
    Task LoadAsync();
    Task<string> GetResponseAsync(string payload);
    Task<string> GetResponseAsync(GeneratePayload payload);
    IAsyncEnumerable<string> GetResponseStream(GeneratePayload payload);
    Task<ModelResponse> GetModels();
    Task<string> GetResponseAsync(ChatPayload payload);
    Task<string> GetResponseAsync(IEnumerable<string> payload);
}

public class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("images")]
    public List<string> Images { get; set; }

    //[JsonPropertyName("tool_calls")]
    //public List<string> ToolCalls { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Format
{
    public string type { get; set; }
    public string description { get; set; }
    public List<string> @enum { get; set; }
}

public class Function
{
    public string name { get; set; }
    public string description { get; set; }
    public Parameters parameters { get; set; }
}

public class Location
{
    public string type { get; set; }
    public string description { get; set; }
}

public class Parameters
{
    public string type { get; set; }
    public Properties properties { get; set; }
    public List<string> required { get; set; }
}

public class Properties
{
    public Location location { get; set; }
    public Format format { get; set; }
}
 

public class Tool
{
    public string type { get; set; }
    public Function function { get; set; }
}



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

public class AdvancedParameters
{
    [JsonPropertyName("options")]
    public Dictionary<string, object> Options { get; set; }

    [JsonPropertyName("keep_alive")]
    public TimeSpan KeepAlive { get; set; }

    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}