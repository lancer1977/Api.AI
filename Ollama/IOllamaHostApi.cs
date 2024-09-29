using Ollama.Models;

namespace Ollama;

public interface IOllamaHostApi
{
    Task<bool> Impersonate(AddMessageParams message);
    Task<bool> AddMessage(AddMessageParams message);
}