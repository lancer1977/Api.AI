using Ollama.Models;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace Ollama.Ollama
{
    public class OllamaServer : IServer
    {
        public OllamaServer(IOllamaService service)
        {
            Service = service;
        }

        public IOllamaService Service { get; init; }

        public Task LoadAsync()=> Service.LoadAsync();
    

        public Task<bool> CheckHealth()=> Service.CheckHealth();
        public Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType request)
        {
            throw new NotImplementedException();
        }

        public   Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType<T> request)
        {
            var payload = request.ToGeneratePayload<T>();
            return Service.GetResponseAsync<T>(payload); 
        }

        public IAsyncEnumerable<string> GetResponseStream(AiRequestType request)
        {
            var payload = request.ToGeneratePayload(true);
            return Service.GetResponseStream(payload);
        }

        public Task<PersonalityType> GetModels()
        {
            throw new NotImplementedException();
        }

        public string Type { get; set; }
    }
}