using Microsoft.Extensions.Configuration;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Test
{
    public class HttpService : IHttpService
    {
        private readonly IConfigurationRoot _configuration;

        public HttpService()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
                .AddUserSecrets("d1921150-b78b-40bf-ba16-9dcf02692536") // Use the UserSecretsId generated earlier
                .Build();
            //var _ownerServiceMoq = new Mock<IIdentityService>();
            var token = _configuration.GetValue("Token", "") ?? "";
            _token = token;
            //_ownerServiceMoq.Setup(p => p.OwnerId).Returns(Guid.Parse(id));
        }
        private string _token;
        public async Task<string> GetAuthToken() => await Task.FromResult(_token);

        public HttpClient GetClient { get; } = new HttpClient();
    }
}