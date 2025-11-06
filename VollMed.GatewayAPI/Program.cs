using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------
// 1. CORS
// -----------------------------------------------------------
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

// -----------------------------------------------------------
// 2. Autenticação e OpenIdConnect
// -----------------------------------------------------------
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:5001"; // Duende IdentityServer
        options.ClientId = "VollMed.Web.Id";          // mesmo ClientId do Identity
        options.ClientSecret = "secret";              // mesmo segredo
        options.ResponseType = "code";
        options.SaveTokens = true;

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("VollMed.Web.Scope");
        options.Scope.Add("Consultas.ServiceAPI.Scope");
        options.Scope.Add("Medicos.ServiceAPI.Scope");
        options.Scope.Add("Pacientes.ServiceAPI.Scope");

        options.GetClaimsFromUserInfoEndpoint = true;

    });


// -----------------------------------------------------------
// 3. YARP — Proxy + Propagação do Token
// -----------------------------------------------------------
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async transformContext =>
        {
            var accessToken = await transformContext.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                transformContext.ProxyRequest.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                Console.WriteLine("Nenhum access_token encontrado no contexto HTTP!");
            }
        });
    });

// -----------------------------------------------------------
// 4. Outros serviços
// -----------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

var app = builder.Build();

// -----------------------------------------------------------
// 5. Pipeline
// -----------------------------------------------------------
app.UseHttpsRedirection();
app.UseCors("AllowMvcApp");

app.UseAuthentication();
app.UseAuthorization();

// Endpoint simples de teste
app.MapGet("/", () => "YARP Gateway funcionando e autenticado com Duende IdentityServer!");

// Proxy — todas as rotas configuradas em appsettings.json
app.MapReverseProxy();

app.Run();
