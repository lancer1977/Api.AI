using Ollama.Payloads;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;

namespace Ollama.Ollama;
    public class OllamaServer : IAIService
    {
        public OllamaServer(IOllamaService service)
        {
            Service = service;
        }

        public IOllamaService Service { get; init; }

        public Task LoadAsync()=> Service.LoadAsync();
    

        public Task<bool> CheckHealth()=> Service.CheckHealth();

        public   Task<AiResponseType<T?>> GetResponseAsync<T>(AiRequestType<T> request)
        {
            var payload = request.ToGeneratePayload<T>();
            return Service.GetResponseAsync<T>(payload); 
        }

        public IAsyncEnumerable<string> GetResponseStream(AiRequestType request)
        {
            var payload = request.ToGeneratePayload();
            return Service.GetResponseStream(payload);
        }

        public async Task<IEnumerable<PersonalityType>> GetModels()
        {
            var models = await Service.GetModels();
            var personalities = models.Models.Select(x => new PersonalityType(x.Name, x.Model, ""));
            return personalities;
        }

        public string Type { get; set; }
    }
