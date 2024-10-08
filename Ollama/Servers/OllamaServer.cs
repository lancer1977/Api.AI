﻿using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.AI.Models;
using PolyhydraGames.Ollama.Payloads;

namespace PolyhydraGames.Ollama.Servers;
    public class WrappedOllamaService : IAIService
    {
        public WrappedOllamaService(IOllamaService service)
        {
            Service = service;
        }

        public IOllamaService Service { get; init; }

        public Task LoadAsync()=> Service.LoadAsync();
    

        public Task<bool> HealthCheck()=> Service.CheckHealth();

        public  async Task<AiResponseType> GetResponseAsync(AiRequestType request)
        {
            var payload = request.ToGeneratePayload();
            var response = await Service.GetResponseAsync(payload);
            return response;
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
