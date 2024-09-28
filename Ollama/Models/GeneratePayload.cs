using System.Text.Json.Serialization;
using PolyhydraGames.AI.Models;

namespace Ollama.Models;
public static class GeneratePayloadExtensions
{
    public static GeneratePayload ToGeneratePayload<T>(this AiRequestType<T> request)
    {
        return new GeneratePayload(request.UserPrompt);
    }
    public static GeneratePayload ToGeneratePayload(this AiRequestType request)
    {
        return new GeneratePayload(request.UserPrompt);
    }
    public static GeneratePayload ToGeneratePayload(this string prompt)
    {
        return new GeneratePayload(prompt);
    }

    public static GeneratePayload WithMinutes(this GeneratePayload payload, int minutes = 10)
    {
        payload.KeepAlive = $"{minutes}m";
        return payload;
    }

    public static GeneratePayload WithHours(this GeneratePayload payload, int hours)
    {
        payload.KeepAlive = $"{hours}m";
        return payload;
    }
}
public record GeneratePayload (string Prompt)
{
    /// <summary>
    /// a duration string (such as "10m" or "24h")
    //a number in seconds(such as 3600)
    //any negative number which will keep the model loaded in memory(e.g. -1 or "-1m")
    //'0' which will unload the model immediately after generating a response
    /// </summary>
    [JsonPropertyName("keep_alive")]
    public string KeepAlive { get; set; } = "10m";
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    [JsonPropertyName("prompt")] 
    public string Prompt { get; set; } = Prompt;
    [JsonPropertyName("format")]
    public string Format { get; set; }
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }

}