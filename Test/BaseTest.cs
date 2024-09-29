using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.AI.Test
{
    public abstract class BaseTest
    {
        protected string WebApiAddress = "https://api.polyhydragames.com/ai";
        //protected string WebApiAddress = "http://192.168.0.21:285";
        //protected readonly string WebApiAddress = "https://localhost:7162";
        protected readonly IEndpointFactory Factory;
        protected readonly IHttpService HttpService;

        public BaseTest()
        {
            var endMock = new Moq.Mock<IEndpointFactory>();
            endMock.Setup(p => p.GetEndpoint()).Returns(WebApiAddress);
            Factory = endMock.Object;
            HttpService = new HttpService();

        }
    }
}