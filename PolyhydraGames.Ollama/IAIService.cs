using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama;

public interface IAIService
{
    Task LoadAsync();
    Task<string> GetResponseAsync(string payload);
    Task<string> GetResponseAsync(GeneratePayload payload);
    Task<string> GetResponseAsync(ChatPayload payload);
    IAsyncEnumerable<string> GetResponseStream(GeneratePayload payload);
    Task<ModelResponse> GetModels();
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);