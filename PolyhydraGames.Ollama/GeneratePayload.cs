using System.Text.Json.Serialization;

namespace PolyhydraGames.Ollama
{
    public class GeneratePayload
    {
        public GeneratePayload()
        {

        }
        public GeneratePayload(string prompt)
        {
            Prompt = prompt;
        }
        /// <summary>
        /// a duration string (such as "10m" or "24h")
        //a number in seconds(such as 3600)
        //any negative number which will keep the model loaded in memory(e.g. -1 or "-1m")
        //'0' which will unload the model immediately after generating a response
        /// </summary>
        [JsonPropertyName("keep_alive")]
        public string KeepAlive { get; set; }
        [JsonPropertyName("model")]
        public string? Model { get; set; }
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
        public void SetDurationFromMinutes(int minutes)
        {
            KeepAlive = $"{minutes}m";
        }

        public void SetDurationFromHours(int hours)
        {
            KeepAlive = $"{hours}h";
        }
    }
}