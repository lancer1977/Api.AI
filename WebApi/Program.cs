using Microsoft.IdentityModel.Tokens;
using PolyhydraGames.AI.WebApi;
using PolyhydraGames.Core.AspNet.IdentityServices;
using PolyhydraGames.Core.AspNet.Middleware.Owner;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Streaming.SQL.Databases;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    //x.JsonSerializerOptions.Converters.Add(new InterfaceListConverter<IDefinition, Definition>());
    //x.JsonSerializerOptions.Converters.Add(new InterfaceIListConverter<IDefinition, Definition>());
}

    );
builder.Services.AddSwaggerGen();
builder.Services.RegisterUserServices(builder.Configuration);
builder.Services.AddScoped<IIdentityService, ApiIdentityService>();
builder.Services.AddScoped<IDBConnectionFactory>(x => new SQLStreamingConnectionFactory(x.GetService<IConfiguration>()?.GetConnString("Streaming", "SqlPassword") ?? throw new NullReferenceException("Streaming factory")));
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        //policy.RequireClaim("scope", "Streaming");
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
app.Run();