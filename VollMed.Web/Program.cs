using VollMed.Web.Filters;
using VollMed.Web.Interfaces;
using VollMed.Web.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ExceptionHandlerFilter>();
});

// Criar HttpClient singleton para cada API
var consultasUrl = builder.Configuration["Services:Consultas"];
if (string.IsNullOrWhiteSpace(consultasUrl))
    throw new Exception("ApiConsultas URL não configurada corretamente.");
var httpClientConsultas = new HttpClient { BaseAddress = new Uri(consultasUrl) };

var medicosUrl = builder.Configuration["Services:Medicos"];
if (string.IsNullOrWhiteSpace(medicosUrl))
    throw new Exception("ApiMedicos URL não configurada corretamente.");
var httpClientMedicos = new HttpClient { BaseAddress = new Uri(medicosUrl) };

var pacientesUrl = builder.Configuration["Services:Pacientes"];
if (string.IsNullOrWhiteSpace(pacientesUrl))
    throw new Exception("ApiPacientes URL não configurada corretamente.");
var httpClientPacientes = new HttpClient { BaseAddress = new Uri(pacientesUrl) };

// Registrar os HttpClients
builder.Services.AddSingleton(httpClientConsultas);
builder.Services.AddSingleton(httpClientMedicos);
builder.Services.AddSingleton(httpClientPacientes);

// Registrar o serviço MedVollApiService com dois HttpClients
builder.Services.AddSingleton<IVollMedApiService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var consultasClient = sp.GetRequiredService<HttpClient>(); // HttpClient de Consultas
    var medicosClient = sp.GetRequiredService<HttpClient>();   // HttpClient de Médicos
    var pacientesClient = sp.GetRequiredService<HttpClient>();   // HttpClient de Médicos
    return new VollMedApiService(config, httpClientConsultas, httpClientMedicos, pacientesClient);
});

builder.Services.AddScoped<ExceptionHandlerFilter>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithReExecute("/erro/{0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages().RequireAuthorization();

app.Run();
