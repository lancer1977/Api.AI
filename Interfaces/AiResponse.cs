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
    public static async Task<AiResponseType<T?>> Create<T>(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<T>(responseBody);
        return ollamaResponse == null ? new AiResponseType<T?>(string.Empty, default) : new AiResponseType<T?>(string.Empty, ollamaResponse);
    }
}