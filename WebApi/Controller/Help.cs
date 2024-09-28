using Ollama;
using Ollama.Ollama;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI.WebApi.Controller
{
    public static class Help
    {
        public static IAIService GetNewService(this IHttpClientFactory factory, ServerDefinitionType def) //add filters
        {
            switch (def.Type)
            {
                case "Ollama":
                    var srv = new OllamaService(factory, def.ToOllamaConfig());
                    return new OllamaServer(srv);
                default:
                    throw new Exception($"Not recognized {def.Type}");
            }
        }
    }
}