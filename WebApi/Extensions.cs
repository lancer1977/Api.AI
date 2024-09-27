using Microsoft.Data.SqlClient;
using PolyhydraGames.Core.Interfaces; 

namespace PolyhydraGames.Ollama.WebApi;

public static class Extensions
{

    public static string GetConnString(this IConfiguration config, string connectionString, string passwordKey)
    {
        var password = config[passwordKey];
        var connString = config.GetConnectionString(connectionString);
        if(string.IsNullOrWhiteSpace(password)) throw new NullReferenceException("Password was null");
        if (string.IsNullOrWhiteSpace(connString)) throw new NullReferenceException("ConnectionString was null");
        var conStrBuilder =
            new SqlConnectionStringBuilder(connString)
            {
                Password = password
            };
        return conStrBuilder.ConnectionString;
    }

    public static void RegisterUserServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var masterConn = configuration.GetConnString("Streaming", "SqlPassword");
        services.AddScoped<IDBConnectionFactory>(x => new SQLStreamingConnectionFactory(masterConn)); 
    }

}I 