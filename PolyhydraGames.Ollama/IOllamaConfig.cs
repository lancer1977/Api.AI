namespace PolyhydraGames.Ollama
{
    public interface IOllamaConfig
    {
        public string ApiUrl { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }
}