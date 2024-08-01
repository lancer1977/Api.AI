using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;


public class OllamaResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("done")]
    public bool? Done { get; set; }

    [JsonPropertyName("done_reason")]
    public string DoneReason { get; set; }

    [JsonPropertyName("context")]
    public List<int>? Context { get; set; }

    [JsonPropertyName("total_duration")]
    public long? TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public double? LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public int? PromptEvalDuration { get; set; }

    [JsonPropertyName("eval_count")]
    public int? EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public long? EvalDuration { get; set; }
}
