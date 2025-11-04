using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcApp", policy =>
    {
        policy
            .WithOrigins("https://localhost:7236", "http://localhost:7236")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// YARP — configuração via appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection(); //  Middleware pipeline — ordem é importante!
app.UseCors("AllowMvcApp"); // Ativar CORS aqui


// Endpoint simples de teste
app.MapGet("/", () => "YARP Gateway funcionando e autenticado com Duende IdentityServer!");

// Proxy — 
app.MapReverseProxy(); 
app.Run();

