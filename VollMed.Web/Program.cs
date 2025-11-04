using Microsoft.AspNetCore.Mvc;
using VollMed.Web.Services;
using System.Net.Http.Headers;
using VollMed.Web.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

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

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowGateway");
app.UseSession();

app.UseAuthorization();

// Rotas padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
