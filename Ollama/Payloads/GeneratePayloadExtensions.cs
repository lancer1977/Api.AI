using PolyhydraGames.AI.Models;

namespace Ollama.Payloads
{
    public static class GeneratePayloadExtensions
    {
        public static GeneratePayload ToGeneratePayload(this AiRequestType request, bool stream = false)
        {
            return new GeneratePayload(request.UserPrompt)
            {
                Stream = stream
            };
        }
        public static GeneratePayload ToGeneratePayload<T>(this AiRequestType<T> request)
        {
            return new GeneratePayload(request.Prompt) { };
        }
 
        public static GeneratePayload ToGeneratePayload(this string prompt)
        {
            return new GeneratePayload(prompt);
        }

        public static GeneratePayload WithMinutes(this GeneratePayload payload, int minutes = 10)
        {
            payload.KeepAlive = $"{minutes}m";
            return payload;
        }

        public static GeneratePayload WithHours(this GeneratePayload payload, int hours)
        {
            payload.KeepAlive = $"{hours}m";
            return payload;
        }
     
    }
}