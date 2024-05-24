using Microsoft.Extensions.Configuration;

namespace PolyhydraGames.RACheevos.Test;
public abstract class BaseTests
{
    private IConfiguration _configuration;
    public ICheevoAuth Config { get; set; }
    protected string TestUser = "MaxMilyin";
    protected int TestGameId = 14402;

    public BaseTests()
    {

        _configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the test project
            .AddUserSecrets("55ffaff5-0bdc-44ea-8c06-273a06fec476") // Use the UserSecretsId generated earlier
            .Build();
        Config = new DefaultAuthConfig()
        {
            ApiKey = _configuration["ApiKey"],
            UserName = _configuration["UserName"]
        };

    }



}