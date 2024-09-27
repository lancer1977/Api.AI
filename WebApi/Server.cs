namespace PolyhydraGames.AI.WebApi
{
    public class Server: IServer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Models { get; set; }
        public int Priority { get; set; }
        public string Availibility { get; set; }
        public int Speed { get; set; }
        public string Address { get; set; }
    }
}