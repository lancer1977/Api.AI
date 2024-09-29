using Microsoft.IdentityModel.Tokens;
using PolyhydraGames.AI.WebApi;
using PolyhydraGames.Core.AspNet.IdentityServices;
using PolyhydraGames.Core.AspNet.Middleware.Owner;
using PolyhydraGames.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();


builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen()
    .AddHttpClient()
    .AddLogging(x =>
    {
        x.AddConsole();
        x.AddSeq(builder.Configuration.GetSection("Seq"));

#if !DEBUG
 x.AddApplicationInsights(configureTelemetryConfiguration: (config) => config.ConnectionString = builder.Configuration.GetConnectionString("MSInsights"), configureApplicationInsightsLoggerOptions: (options) => { });
#endif
    })
    .AddScoped<IIdentityService, ApiIdentityService>()
    .AddSingleton<IServerSource, ServerSource>();
//builder.Services.AddScoped<IDBConnectionFactory>(x => new SQLAIConnectionFactory(x.GetService<IConfiguration>()?.GetConnString("AI", "SqlPassword") ?? throw new NullReferenceException("AI factory")));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        //policy.RequireClaim("scope", "AI");
    });
});
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["Identity:Authority"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });
builder.Services.AddControllers().AddJsonOptions(x =>
{
    //x.JsonSerializerOptions.Converters.Add(new InterfaceListConverter<IDefinition, Definition>());
    //x.JsonSerializerOptions.Converters.Add(new InterfaceIListConverter<IDefinition, Definition>());
});
//builder.ConfigureAuthsenticationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseOwnerMiddleware();
app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();
app.MapHealthChecks("/healthcheck");
await ServerSource.InitializeAsync(app);
app.Run();

