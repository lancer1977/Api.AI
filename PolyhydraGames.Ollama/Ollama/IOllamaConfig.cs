namespace PolyhydraGames.Ollama.Ollama
{
    public interface IOllamaConfig
    {
        public string ApiUrl { get; set; }
        public string Background { get; set; }
        public string Key { get; set; }
    }
}