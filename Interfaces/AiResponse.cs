using System.Text.Json;
using PolyhydraGames.AI.Models;

namespace PolyhydraGames.AI;

public static class AiResponse
{
    public static AiResponseType<T> Create<T>(string rawResponse)
    {
        var result = new AiResponseType<T>(rawResponse, JsonSerializer.Deserialize<T>(rawResponse));
        return result;
    }
}