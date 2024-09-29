using System.ComponentModel;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Payloads;

public record GeneratePayload<T>(string Prompt): GeneratePayload(Prompt);
public record GeneratePayload(string Prompt)
{
    /// <summary>
    /// a duration string (such as "10m" or "24h")
    //a number in seconds(such as 3600)
    //any negative number which will keep the model loaded in memory(e.g. -1 or "-1m")
    //'0' which will unload the model immediately after generating a response
    /// </summary>
    [JsonPropertyName("format")]
    public string? Format { get; set; } = "";

    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("options")]
    public string? Options { get; set; }

    [JsonPropertyName("system")]
    [Description("system message to (overrides what is defined in the Modelfile)")]
    public string? System { get; set; }
    [JsonPropertyName("prompt")] 
    public string Prompt { get; set; } = Prompt;

    [JsonPropertyName("suffix")] 
    public string Suffix { get; set; } = "";

    [JsonPropertyName("template")]
    public string? Template { get; set; }

    [JsonPropertyName("context")]
    public int? Context { get; set; }
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    [JsonPropertyName("raw")]
    public bool Raw { get; set; }
    [JsonPropertyName("keep_alive")]
    public string KeepAlive { get; set; } = "10m";

}