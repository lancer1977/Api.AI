using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama;


//public class OllamaResponse
//{
//    [JsonPropertyName("model")]
//    public string Model { get; set; }
//    [JsonPropertyName("created_at")]
//    public DateTime CreatedAt { get; set; }
//    [JsonPropertyName("response")]
//    public string Response { get; set; }
//    [JsonPropertyName("done")]
//    public bool Done { get; set; }
//}
 
public class OllamaResponse
{
    [JsonPropertyName("model")]
    public string Model;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("response")]
    public string Response;

    [JsonPropertyName("done")]
    public bool Done;

    [JsonPropertyName("done_reason")]
    public string? DoneReason;

    [JsonPropertyName("context")]
    public List<int?>? Context;

    [JsonPropertyName("total_duration")]
    public long? TotalDuration;

    [JsonPropertyName("load_duration")]
    public int? LoadDuration;

    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount;

    [JsonPropertyName("prompt_eval_duration")]
    public int? PromptEvalDuration;

    [JsonPropertyName("eval_count")]
    public int? EvalCount;

    [JsonPropertyName("eval_duration")]
    public long? EvalDuration;
}