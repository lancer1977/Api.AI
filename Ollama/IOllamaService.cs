using PolyhydraGames.AI.Models;
using PolyhydraGames.Ollama.Models;
using PolyhydraGames.Ollama.Payloads;

namespace PolyhydraGames.Ollama;

public interface IOllamaService
{
    Task LoadAsync();
    Task<bool> CheckHealth();
    Task<AiResponseType> GetResponseAsync(GeneratePayload payload);
    Task<AiResponseType> GetResponseAsync(ChatPayload payload); 
    Task<AiResponseType> GetResponseAsync(string payload); 
    IAsyncEnumerable<string> GetResponseStream(GeneratePayload payload);
    Task<ModelResponse> GetModels();
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);