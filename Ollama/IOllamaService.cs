using Ollama.Models;
using Ollama.Payloads;
using PolyhydraGames.AI.Models;

namespace Ollama;

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