using Ollama.Models;
using Ollama.Ollama;
using PolyhydraGames.AI.Models;

namespace Ollama;

public interface IOllamaHostApi
{
    Task<bool> Impersonate(AddMessageParams message);
    Task<bool> AddMessage(AddMessageParams message);
}

public static class ServerDefinitionExtensions
{
    public static IOllamaConfig ToOllamaConfig(this ServerDefinitionType server)
    {
        return new OllamaConfig()
        {
            ApiUrl = server.Address,
            Background = server.Description,
            //Key = server.Id
        };

    }
}