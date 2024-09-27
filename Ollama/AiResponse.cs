using System.Text.Json;
using Ollama.Models;

namespace Ollama
{
    public static partial class AiResponse
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
                return Create<T?>(ollamaResponse.Response);
            }
        }


    }
}