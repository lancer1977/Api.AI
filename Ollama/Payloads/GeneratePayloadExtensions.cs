﻿using PolyhydraGames.AI.Models;
using System.IO;

namespace Ollama.Payloads
{
    public static class GeneratePayloadExtensions
    {
        public static GeneratePayload ToGeneratePayload(this AiRequestType request)
        {
            return new GeneratePayload(request.Prompt)
            {
                Stream = request.Stream,
                ModelName = request.ModelName,
                Options = request.Options,
                Context = request.Context,
                Template = request.Template,
                System = request.System,
                Suffix = request.Suffix,
                Raw = request.Raw

            };
        }
        public static GeneratePayload ToGeneratePayload<T>(this AiRequestType<T> request)
        {
            return new GeneratePayload<T>(request.Prompt)
            {
                Stream = request.Stream,
                ModelName = request.ModelName,
                Options = request.Options,
                Context = request.Context,
                Template = request.Template,
                System = request.System,
                Suffix = request.Suffix,
                Raw = request.Raw

            };
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