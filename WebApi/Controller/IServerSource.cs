namespace PolyhydraGames.AI.WebApi.Controller
{
    public interface IServerSource
    {
        Task AddOrUpdateServer(Server server);
        Task<IEnumerable<IServer>> Items();
    }
}