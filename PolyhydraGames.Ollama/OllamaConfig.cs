namespace PolyhydraGames.Ollama
{
    public class OllamaConfig : IOllamaConfig
    {
        public string ApiUrl { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}