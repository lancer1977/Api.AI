using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PolyhydraGames.AI.Test
{
    public static class Fixture
    { 
        public static IHost Create(Action<IServiceCollection> registrations)
        {
            // Arrange: Create the HostBuilder
            var hostBuilder = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    registrations.Invoke(services);
                    // Register the services for testing
                    //services.AddTransient<IMyService, MyService>();
                    //services.AddSingleton<IOtherDependency, MockOtherDependency>();
                });
            return hostBuilder.Build();
        }

        public static async Task<IHost> Run(this IHost host, Func<IServiceProvider, Task> act)
        {    
            await host.StartAsync();
            await act.Invoke(host.Services);
            return host;
        }
    }
}