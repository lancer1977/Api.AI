using System.Text.Json;
using Ollama.Models;
using PolyhydraGames.AI.Models;

namespace Ollama;

public static   class AiResponse
{


    public static async Task<AiResponseType<T?>> Create<T>(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        if (ollamaResponse == null)
        {
            return new AiResponseType<T?>(string.Empty, default);
        }
        else
        {
            return PolyhydraGames.AI.Models.AiResponse.Create<T?>(ollamaResponse.Response);
        }
    }


}
