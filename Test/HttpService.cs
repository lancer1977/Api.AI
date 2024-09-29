using Microsoft.Extensions.Configuration;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Test;
public class HttpService : IHttpService
{
    public HttpService(IConfiguration config)
    {
        var token = config.GetValue("Token", "") ?? "";
        _token = token;
        //_ownerServiceMoq.Setup(p => p.OwnerId).Returns(Guid.Parse(id));
    }
    private string _token; 
    public async Task<string> GetAuthToken() => await Task.FromResult(_token);

    public HttpClient GetClient { get; } = new HttpClient();
}
