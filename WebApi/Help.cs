﻿using PolyhydraGames.Ollama;
using PolyhydraGames.Ollama.Servers;

namespace PolyhydraGames.AI.WebApi;
public static class Help
{
    public static async Task<IAIService> GetNewService(this IHttpClientFactory factory, ServerDefinitionType def, ILogger<OllamaService> logger) //add filters
    {
        switch (def.Type)
        {
            case "Ollama":
                var srv = new OllamaService(factory, def.ToOllamaConfig("ollama3.1:latest"), logger);
                var server = new WrappedOllamaService(srv);
                await server.Initialize(def);
                return server;
            default:
                throw new Exception($"Not recognized {def.Type}");
        }
    }

    public static async Task Initialize(this IAIService source, ServerDefinitionType definition)
    {
        try
        {
            await source.LoadAsync();
            definition.Available = true;
        }
        catch (Exception ex)
        {
            definition.Available = false;
        }
    }
}
