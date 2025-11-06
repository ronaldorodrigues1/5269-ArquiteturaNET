using Microsoft.AspNetCore.Mvc;
using VollMed.Web.Services;
using System.Net.Http.Headers;
using VollMed.Web.Interfaces;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// Adiciona suporte ao IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();

// Configurações da URL do Gateway
var gatewayUrl = builder.Configuration["Services:GatewayYarp"];
if (string.IsNullOrWhiteSpace(gatewayUrl))
    throw new Exception("ApiGateway URL não configurada corretamente.");

// Injeção do HttpClient via IHttpClientFactory
builder.Services.AddHttpClient<IVollMedApiService, VollMedApiService>((sp, client) =>
{
    client.BaseAddress = new Uri(gatewayUrl);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Session
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// CORS (se quiser permitir chamadas cruzadas)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGateway", policy =>
    {
        policy
            .WithOrigins(gatewayUrl)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Logging (mantém igual)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

//Configura o Identity para realizar o login
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "VollMedAuthCookie";
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    }
    )
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["Services:IdentityServer"];

        options.ClientId = "VollMed.Web.Id";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.SaveTokens = true;
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("VollMed.Web.Scope");

        //adiciona o escopo das apis para tratar no gateway
        options.Scope.Add("Consultas.ServiceAPI.Scope");
        options.Scope.Add("Medicos.ServiceAPI.Scope");
        options.Scope.Add("Pacientes.ServiceAPI.Scope");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.MapInboundClaims = false;
        options.SaveTokens = true;
    });

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowGateway");
app.UseSession();

app.UseAuthentication(); // importante a ordem: autenticação primeiro
app.UseAuthorization(); // autorização depois.

app.MapDefaultControllerRoute().RequireAuthorization();

// Rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
