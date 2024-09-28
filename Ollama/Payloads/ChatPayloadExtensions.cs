using Ollama.Models;
using PolyhydraGames.AI.Models;

namespace Ollama.Payloads
{
    public static class ChatPayloadExtensions
    {
        public static ChatPayload ToChatPayload<T>(this AiRequestType request)
        {

            var messages = new List<ChatMessage>(request.additionalMessages.Select(x => new ChatMessage() { Content = x }));
            var payload = new ChatPayload()
            {
                Messages = messages,
                Format = "json",
                Model = request.UserPrompt,
                Stream = false,

            };
            return payload;
        }
        public static ChatPayload ToChatPayload(this AiRequestType request)
        {

            var messages = new List<ChatMessage>(request.additionalMessages.Select(x => new ChatMessage() { Content = x }));
            var payload = new ChatPayload()
            {
                Messages = messages,
                Model = request.UserPrompt,
                Stream = false,

            };
            return payload;
        }
    }
}