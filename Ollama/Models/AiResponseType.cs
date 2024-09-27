using Ollama.Models;
using System.Text.Json;

public static partial class AiResponse
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
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
        if (ollamaResponse == null)
        {
            return new AiResponseType<T?>(string.Empty, default);
        }
        else
        {
            return Create<T?>(ollamaResponse.Response);
        }
    }


}