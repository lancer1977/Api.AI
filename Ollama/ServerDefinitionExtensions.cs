using Ollama.Ollama;
using PolyhydraGames.AI.Models;

namespace Ollama;

public static class ServerDefinitionExtensions
{
    public static IOllamaConfig ToOllamaConfig(this ServerDefinitionType server, string defaultModel)
    {
        return new OllamaConfig()
        {
            ApiUrl = server.Address,
            Background = server.Description,
            DefaultModel = server.DefaultModel,
            //Key = server.Id
        };

    }
}