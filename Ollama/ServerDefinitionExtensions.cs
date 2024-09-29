using PolyhydraGames.AI.Models;
using PolyhydraGames.Ollama.Servers;

namespace PolyhydraGames.Ollama;

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