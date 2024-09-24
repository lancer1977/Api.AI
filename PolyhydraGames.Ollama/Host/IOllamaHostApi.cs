using PolyhydraGames.Ollama.Models;

namespace PolyhydraGames.Ollama.Host
{
    public interface IOllamaHostApi
    {
         Task<bool> Impersonate(AddMessageParams message);
        Task<bool> AddMessage(AddMessageParams message);
    }
}