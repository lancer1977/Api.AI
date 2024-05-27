using Microsoft.Extensions.Configuration;
using Moq;
using PolyhydraGames.Ollama;

namespace PolyhydraGames.RACheevos.Test;
public abstract class BaseTests
{
    private IConfiguration _configuration;
    public IOllamaConfig Config { get; set; }
    public IOllamaService Service { get; set; } 
    public IHttpClientFactory HttpClientFactory { get; set; }

    public BaseTests()
    {

        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
            .AddUserSecrets("65a2f916-1765-44e8-8d59-2d2ddcd7cc9b") // Use the UserSecretsId generated earlier
            .Build();
        Config = new OllamaConfig()
        {
            ApiUrl = _configuration["Ollama:ApiUrl"],
            Key = _configuration["Ollama:Key"],
            Name = _configuration["Ollama:Name"]
        };
        var mock = new Mock<IHttpClientFactory>();
        mock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        HttpClientFactory = mock.Object;
    }

    public class OllamaServiceTests : BaseTests
    {
        [Test]
        public async Task GetOllamaResponse()
        {
            Service = new OllamaService(HttpClientFactory, Config);
            var response = await Service.GetOllamaResponse("What is water made of?");
            Assert.That(response != null);
        }

        [Test]
        public async Task GetOllamaResponseList()
        {
            Service = new OllamaService(HttpClientFactory, Config);
            var response = await Service.GetOllamaListResponse("What is water made of?");
            Assert.That(response != null);
        }
    }


}