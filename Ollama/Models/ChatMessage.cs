﻿using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama.Models;

public class ChatMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("images")]
    public List<string> Images { get; set; } 
}