using Microsoft.Extensions.Configuration;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Test;

public abstract class BaseTest
    { 
        protected string WebApiAddress = "https://api.polyhydragames.com/ai";
        //protected string WebApiAddress = "http://192.168.0.21:285";
        //protected readonly string WebApiAddress = "https://localhost:7162";
        protected readonly IEndpointFactory Factory;
        protected readonly IHttpService HttpService;
        protected readonly IConfigurationRoot _configuration;

        public BaseTest()
        { 
            var endMock = new Moq.Mock<IEndpointFactory>();
            endMock
                .Setup(p => p.GetEndpoint())
                .Returns(WebApiAddress);
            Factory = endMock.Object; 
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
                .AddUserSecrets("d1921150-b78b-40bf-ba16-9dcf02692536") // Use the UserSecretsId generated earlier
                .Build();
            HttpService = new HttpService(_configuration);

        }
    }
