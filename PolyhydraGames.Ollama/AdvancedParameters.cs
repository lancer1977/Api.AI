using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama
{
    public class AdvancedParameters
    {
        [JsonPropertyName("options")]
        public Dictionary<string, object> Options { get; set; }

        [JsonPropertyName("keep_alive")]
        public TimeSpan KeepAlive { get; set; }

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }
}