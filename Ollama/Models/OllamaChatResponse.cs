using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;

public class OllamaChatResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }  
 
    [JsonPropertyName("done_reason")]
    public string DoneReason { get; set; } 
 
         
    [JsonPropertyName("total_duration")]
    public int TotalDuration { get; set; } 
    [JsonPropertyName("load_duration")]
    public int LoadDuration { get; set; }
         
    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }
         
    [JsonPropertyName("prompt_eval_duration")]
    public long PromptEvalDuration { get; set; }
         
    [JsonPropertyName("eval_count")]
    public int EvalCount { get; set; }
         
    [JsonPropertyName("eval_duration")]
    public int EvalDuration { get; set; }

}