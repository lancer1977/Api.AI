using Microsoft.Extensions.Configuration;
using Moq;
using PolyhydraGames.Ollama.Ollama;

namespace PolyhydraGames.Ollama.Test;

public class ChatGPTServiceTests
{
    private IConfiguration _configuration;
    public IOllamaConfig Config { get; set; }
    public IAIService Service { get; set; }
    public IHttpClientFactory HttpClientFactory { get; set; }
    [Test]
    public async Task GetResponse()
    {
            Service = new OllamaService(HttpClientFactory, Config);
            var response = await Service.GetResponseAsync("What is water made of?");
            Assert.That(response != null);
        }
    public ChatGPTServiceTests()
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
                Background = _configuration["Ollama:Background"]
            };
            var mock = new Mock<IHttpClientFactory>();
            mock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            HttpClientFactory = mock.Object;
        }


}