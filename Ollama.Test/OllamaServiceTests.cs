using System.Diagnostics; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PolyhydraGames.AI.Interfaces;
using PolyhydraGames.Ollama.Host;
using PolyhydraGames.Ollama.Models;
using PolyhydraGames.Ollama.Payloads;
using PolyhydraGames.Ollama.Servers;

namespace PolyhydraGames.Ollama.Test;

[TestFixture]
public class OllamaServiceTests
{
    private IConfiguration _configuration;
    public IOllamaConfig Config { get; set; }
    public IOllamaHostSiteConfig HostConfig { get; set; }
    public IOllamaHostApi HostApi { get; set; }
    public OllamaService Service { get; init; } 
    public IHttpClientFactory HttpClientFactory { get; set; }
    public ILoggerFactory _loggerFactory;
    private string DefaultModel = "llama3.2";
    [OneTimeTearDown]
    public void Cleanup()
    {
        _loggerFactory.Dispose();
        _loggerFactory = null;
    }
    [TestCase("Tell everyone you love them")]
    public async Task TestAddMessage(string message)
    {
        var msg = new AddMessageParams("DreadBreadcrumb", message);
        var result = await HostApi.AddMessage(msg);
        Assert.That(result);
    }
    [TestCase("llama3.2", "Who is the best band ever?")]
    public async Task GetOllamaResponse(string model, string payload)
    { 
        var response = await Service.GetResponseAsync(new GeneratePayload(payload,model));
        Assert.That(response != null);
    }
    [Test]
    public async Task GetResponse()
    { 
        var response = await Service.GetResponseAsync("What is water made of?");
        Assert.That(response != null);
    }
    [Test]
    public async Task CheckHeath()
    { 
        var response = await Service.CheckHealth();
 
        Assert.That(response);
    }
    [Test]
    public async Task GetOllamaResponseList()
    {
        var response = Service.GetResponseStream(new GeneratePayload(DefaultModel, "What do you think of modern punk like the green day?"));
        await foreach (var item in response)
        {
            Debug.Write(item);
            Console.Write(item);
        }
        Assert.That(response != null);
    }
    [TestCase("Lancero", "Nirvana - Heart Shaped Box", 80)]
    public async Task GetOllamaChat(string user, string song, int popularity)
    { 
        await ((OllamaService)Service).LoadAsync();
        var response = await Service.GetResponseAsync(new ChatPayload()
        {
            Model = "dj-spotabot:latest",
            Messages = new List<ChatMessage>()
            {
                new ChatMessage()
                {
                    Role="system",
                    Content = $"Thus far this is what has been said in the chat: \n@MESSAGES\n\nRules to follow in response message section:\n1. Any messages prefixed with dreadbread_bot: are from you the bot. \n2. Don't talk to yourself. \n3. Try to use the names of people who have responded except for #1. Keep responses between 100 to 450 characters max. \n4. Try to respond to any users who have responded to you as well when determining the response. \n5. To direct a message at them add a @ to the front of their name.\n6. Response format MUST be 100% JSON encoded. \nResponse Types:\n0.if you just issued a command with the jukebox bot, tell us about the playing song and why you picked it.\n1.pick a song for the occasion\n2.write a response in the context that is slightly antagonistic.\n3.Write a response in the context that is friendly and contributive.\n4.If there was no messages talk about one of the following scenarios: Your a dungeon master telling us about some wild battle, tell us about a cool video game from the retro era as if you were a commercial announcer, talk about some cool comic you read from back in the day.",
                },
                new ChatMessage()
                {
                    Role="assistant",
                    Content = $"What do you think of {user} and their song {song} which is {popularity} popular?",
                },new ChatMessage()
                {
                    Role="user",
                    Content = $"What do you think of {user} and their song {song} which is {popularity} popular?",
                },new ChatMessage()
                {
                    Role="user",
                    Content = $"What do you think of {user} and their song {song} which is {popularity} popular?",
                },
            }

        });
 
            Debug.Write(response);
            Console.Write(response); 
        Assert.That(response != null);
    }
    [TestCase("Lancero","Nirvana - Heart Shaped Box", 80)]
    public async Task GetOllamaResponseList(string user,string song, int popularity)
    { 
        var response = Service.GetResponseStream(new GeneratePayload(DefaultModel, $"What do you think of {user} and their song {song} which is {popularity} popular?"));
 
        await foreach (var item in response)
        {
            Debug.Write(item);
            Console.Write(item);
        }
        Assert.That(response != null);
    }
    public OllamaServiceTests()
    {
        
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
            .AddUserSecrets("65a2f916-1765-44e8-8d59-2d2ddcd7cc9b") // Use the UserSecretsId generated earlier
            .Build();
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider());
        HostConfig = new OllamaHostSiteConfig()
        {
            Url = _configuration["Host:Url"],
            WebKey = _configuration["Host:WebKey"]
        };
        Config = new OllamaConfig()
        {
            ApiUrl = _configuration["Ollama:ApiUrl"],
            DefaultModel = _configuration["Ollama:Key"],
            Background = _configuration["Ollama:Background"]
        };


        var mock = new Mock<IHttpClientFactory>();
        mock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        HttpClientFactory = mock.Object;

        HostApi = new OllamaHostApi(HostConfig);

        Service = new OllamaService(HttpClientFactory, Config, _loggerFactory.CreateLogger< OllamaService>());
    }

}
 
